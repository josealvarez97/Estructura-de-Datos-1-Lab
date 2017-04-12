using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataStructuresURL_2._0
{
    class BTree<Tkey, TValue> where Tkey : IComparable<Tkey>/*, IEnumerable<Tkey>*/
    {
        StreamWriter TreeFile;
        int order;
        int minimumDegree_t;
        int size;
        int height;

        Node<Tkey, TValue> root;

        const int HeaderNumLines = 5;

        public BTree(int order)
        {
            TreeFile = new StreamWriter("~/TreeFolder/BTree.txt");
            this.order = order;
            this.minimumDegree_t = order / 2;
            this.size = 0;
            this.height = 0;

            TreeFile.WriteLine(int.MinValue); // Apuntador A Raiz
            TreeFile.WriteLine(int.MinValue); // Apuntador A Ultima Posicion Vacia
            TreeFile.WriteLine(size); // Tamaño
            TreeFile.WriteLine(order); // Orden
            TreeFile.WriteLine(height);
        }


        public void Insert(Node<Tkey, TValue> node)
        {

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentNode_X">Un nodo "padre" que se asume que esta No Lleno</param>
        /// <param name="index">Posicion en el padre donde esta el hijo que esta lleno</param>
        /// <param name="childNode_y">El hijo de x que esta lleno</param>
        public void SplitChild(Node<Tkey, TValue> parentNode_X, int index, Node<Tkey, TValue> childNode_y)
        {
            Node<Tkey, TValue> newChildNode_z = new Node<Tkey, TValue>();

            newChildNode_z.elements.Capacity = minimumDegree_t - 1;


            // Aqui añadimos los elementos que corresponden al nodo z
            for (int j = 0; j < minimumDegree_t - 1/*Porque solo caben 2t - 1 elementos*/; j++)
                newChildNode_z.elements[j] = childNode_y.elements[j + minimumDegree_t];

            // Si no tratamos con nodos hoja, tambien añadimos los hijos que corresponden al nodo z
            if (!childNode_y.IsLeaf())
                for (int j = 0; j < minimumDegree_t/*Porque caben 2t hijos*/; j++)
                    newChildNode_z.children[j] = childNode_y.children[j + minimumDegree_t];

            childNode_y.elements.Capacity = minimumDegree_t - 1;//n[y] <- t - 1

            //Pasamos algunos hijos a otro intervalo creo... ahh! Es correr los hijos de lugar para meter el hijo z
            for (int j = parentNode_X.elements.Count + 1; j > index; j--) //for j ← n[x] + 1 downto i + 1
                parentNode_X.children[minimumDegree_t + j] = parentNode_X.children[j];//do cj+1[x] ← cj [x]

            // Metemos hijo z en espacio que quedo
            parentNode_X.children[index + 1] = newChildNode_z; //c_i+1[x] ← z

            //Hacemos espacio para el elemento que va a subir al nodo padre
            for (int j = parentNode_X.elements.Count; j >= index; j--) //for j ← n[x] downto i
                parentNode_X.elements[j + 1] = parentNode_X.elements[j]; //do key j+1[x] ← key j [x]

            parentNode_X.elements[index] = childNode_y.elements[minimumDegree_t]; //keyi [x] ← keyt [y]

            parentNode_X.elements.Capacity++; // n[x] ← n[x] + 1



            DiskWrite(childNode_y);
            DiskWrite(newChildNode_z);
            DiskWrite(parentNode_X);
            

            // Nodo x modificado
            // ModificarNodo()

            // Nodo y modificado

            // Nodo z es nuevo

            // Conclusion: Moficar archivo


        }





        //B-TREE-INSERT(T,k)
        /// <summary>
        /// We insert a key k into a B-tree T of height h in a single pass down the tree, requiring
        /// O(h) disk accesses... (...)
        /// The B-TREE-INSERT procedure uses B-TREE-SPLIT-CHILD to guarantee that the recursion never 
        /// descends to a full node.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="key"></param>
        void Insert(Tkey key)
        {
            Node<Tkey, TValue> r = root; //r ← root[T ]

            if (r.elements.Count == 2 * (minimumDegree_t) - 1)
            {
                Node<Tkey, TValue> s = new Node<Tkey, TValue>(); //then s ← ALLOCATE-NODE()

                r = s; //root[T ]← s

                //leaf [s] ← FALSE. No se usará
                s.elements.Capacity = 0;//n[s] ← 0

                s.children[0] = r; //c1[s] ← r


                SplitChild(s, 0, r); //B-TREE-SPLIT-CHILD(s, 1, r)
                InsertNonFull(s, key); //B-TREE-INSERT-NONFULL(s, k)
            }
            else
            {
                InsertNonFull(r, key);
            }

        }

        public void InsertNonFull(Node<Tkey, TValue> Node_x, Tkey key)
        {
            int i = Node_x.elements.Count; //i ← n[x]. 

            if (Node_x.IsLeaf())
            {
                // Correr los elementos hasta encontrar un elemento mayor
                while(i >= 0 /*Para tomar en cuenta el primer elemento de la lista*/
                    && key.CompareTo(Node_x.elements[i].key) == -1)
                {
                    Node_x.elements[i + 1].key = Node_x.elements[i].key;
                    i--;
                }
                DiskWrite(Node_x);
            }
            else //En este caso no es un nodo hoja
            {
                while (i >= 0
                    && key.CompareTo(Node_x.elements[i].key) == -1)
                {
                    //Retrocedemos el contador hasta encontrar... no se
                    i--;
                }
                i++;

                DiskRead(Node_x.children[i]);

                if(Node_x.children[i].elements.Count == (2 * minimumDegree_t - 1))
                {
                    SplitChild(Node_x, i, Node_x.children[i]);
                    if(key.CompareTo(Node_x.elements[i].key) == 1)
                    {
                        i++;
                    }
                }
                InsertNonFull(Node_x.children[i], key);

            }
        }






        public void DiskWrite(Node<Tkey, TValue> node)
        {
            if (NodeExistsInDisk(node))
            {
                //Pos modificamos
                
            }
            else
            {
                //Pos agregamos uno nuevo
            }
        }
        public void DiskRead(Node<Tkey, TValue> node)
        {

        }

        public bool NodeExistsInDisk(Node<Tkey, TValue> node)
        {
            return false;
        }
        public bool NodeExistsInDisk(Tkey key)
        {
            return false;
        }


    }
}
