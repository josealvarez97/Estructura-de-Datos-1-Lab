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



            // Nodo x modificado
            // ModificarNodo()

            // Nodo y modificado

            // Nodo z es nuevo

            // Conclusion: Moficar archivo


        }





        public void DiskWrite(Node<Tkey, TValue > node, string pointer)
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
