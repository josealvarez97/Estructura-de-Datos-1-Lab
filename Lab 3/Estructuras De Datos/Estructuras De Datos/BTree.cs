using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estructuras_De_Datos
{
    public class BTree<Tkey, Tpointer> where Tkey : IComparable<Tkey>, IEnumerable<Tkey>
    {
        /*There are lower and upper bounds on the number of keys a node can contain.
         *These bounds can be expressed in termes of a fixed integer t >= 2 called the
         *minimum degree of the B-tree
        */
        int minimumDegree;

        public BTree(int minimumDegree)
        {
            this.minimumDegree = minimumDegree;
        }

        Node<Tkey, Tpointer> root;


        // B-TREE-SEARCH(x,k)
        /// <summary>
        /// B-TREE SEARCH is a straightforward generalization of the TREE-SEARCH 
        /// procedure defined for binary search trees (...)
        /// 
        /// B-TREE SEARCH takes as input a POINTER TO THE ROOT NODE x of as subtree and
        /// a KEY k (or "keyToSearch", better) to be searched for in that subtree.
        /// 
        /// The top level call is thus of the form B-TREE-SEARCH(root, keyToSearch)
        /// 
        /// If keyToSearch is in the B-Tree, B-TREE-SEARCH returns 
        /// the ordered pair (NODE y, INDEX i) consisting of a node y and an index i.
        ///     Such that key_i(y) = keyToSearch
        /// </summary>
        /// <param name="keyToSearch">The key to search</param>
        /// <returns></returns>
        Element<Tkey, Tpointer> Search(Tkey keyToSearch)
        {
            return root.Search(root, keyToSearch);
        }

        // B-TREE-CREATE(T)
        /// <summary>
        /// To build a B-tree T, we first use B-Tree-Create to create an empty root node
        /// and then call B-Tree-Insert to add new keys. Both of these procedures use an 
        /// auxiliary procedure ALLOCATE-NODE, which allocates one disk page to be used
        /// as a new node in O(1) time. We can assume that a node created by ALLOCATE-NODE
        /// requires no DISK-READ, since there is as yet no useful information stored on the
        /// disk for that node.
        /// </summary>
        void Create(Tpointer value)
        {
            Node<Tkey, Tpointer> node_x = new Node<Tkey, Tpointer>(); // x <- allocate-node

            //------------------No on this implementation-----------------
            //leaf[x] <- true
            // n[x] <- 0
            // Disk-Write(x)
            //------------------------------------------------------------

            root = node_x;

            //B-Tree-Create requires O(1) disk operations and O(1) CPU time.


        }



        // B-TREE-SPLIT-CHILD(x, i, y)
        /// <summary>
        /// The procedure B-Tree-Split-Child takes as input a nonfull internal node x
        /// (assumed to be in main memory), an index i, and a node y (also assumed to be in main memory)
        /// such that y = c_i[x] is a full child of x. The procedure then splits this child in two and 
        /// adjusts x so that it has an additional child. (To split a full root, we will first make the 
        /// root a child of a new empty root node, so that we can use B-Tree-Split-Child.)
        /// (...) the full node y is split about its median key S, which is moved up into y's parent node x.
        /// Those keys in y that are greater than the median key are placed in a new node z, which is made a new
        /// child of x.
        /// </summary>
        /// <param name="parentNode_X"></param>
        /// <param name="index"></param>
        /// <param name="childeNode_y"></param>
        void SplitChild(Node<Tkey, Tpointer> parentNode_X, int index, Node<Tkey, Tpointer> childeNode_y)
        {
            Node<Tkey, Tpointer> newChildNode_z = new Node<Tkey, Tpointer>(); //Allocate-Node()

            // leaf[z] <- leaf[y]. Este si no creo que lo necesite...

            newChildNode_z.elements.Capacity = minimumDegree - 1;// n[z] <- t-1

            // Aqui añadimos los elementos que corresponden al node z.
            for (int j = 0; j < minimumDegree - 1 /*Porque solo caben 2t - 1 elementos*/; j++) // for j <- 1 to t-1
                //Esto va a dar error porque la lista no esta inicializada en el tamaño que es, para eso es n[z] <- t - 1. Listo, agregue .capacity (...)
                newChildNode_z.elements[j] = childeNode_y.elements[j + minimumDegree];


            // Si no tratamos con nodos hoja, también añadimos los hijos que corresponden al node z
            if(!childeNode_y.IsLeaf())
                for (int j = 0; j < minimumDegree; j++)
                    newChildNode_z.children[j] = childeNode_y.children[j + minimumDegree];

            childeNode_y.elements.Capacity = minimumDegree - 1;//n[y] <- t - 1

            //Pasamos algunos hijos a otro intervalo creo... ahh! Es correr los hijos de lugar para meter el hijo z
            for (int j = parentNode_X.elements.Count + 1; j > index; j--) //for j ← n[x] + 1 downto i + 1
                parentNode_X.children[minimumDegree + j] = parentNode_X.children[j];//do cj+1[x] ← cj [x]

            // Metemos hijo z en espacio que quedo
            parentNode_X.children[index + 1] = newChildNode_z; //c_i+1[x] ← z

            //Hacemos espacio para el elemento que va a subir al nodo padre
            for (int j = parentNode_X.elements.Count; j >= index; j--) //for j ← n[x] downto i
                parentNode_X.elements[j + 1] = parentNode_X.elements[j]; //do key j+1[x] ← key j [x]

            parentNode_X.elements[index] = childeNode_y.elements[minimumDegree]; //keyi [x] ← keyt [y]

            parentNode_X.elements.Capacity++; // n[x] ← n[x] + 1

            // DISK-WRITE (y)
            // DISK-WRITE (z)
            // DISK-WRITE (x)



            /* This method works by straightforward "cutting and pasting." Here, y is the ith child of x and 
             * is the node being split. Node y originally has 2t children (2t - 1 keys) but is reduced to t 
             * children (t - 1 keys) by this operation. Node z "adopts" the t largest children (t - 1 keys) of y, 
             * and z becomes a new child of x, positioned just after y in x's table of children. The median key of y
             * moves up to become the key in x that separates y and z.
             */

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
        void Insert(BTree<Tkey,Tpointer> Tree, Tkey key)
        {
            Node<Tkey, Tpointer> r = Tree.root; //r ← root[T ]

            if(r.elements.Count == 2*(minimumDegree) - 1)
            {
                Node<Tkey, Tpointer> s = new Node<Tkey, Tpointer>(); //then s ← ALLOCATE-NODE()

                r = s; //root[T ]← s

                //leaf [s] ← FALSE. No se usará
                s.elements.Capacity = 0;//n[s] ← 0

                r = s.children[0]; //c1[s] ← r


                SplitChild(s, 0, r); //B-TREE-SPLIT-CHILD(s, 1, r)
                InsertNonFull(s, key); //B-TREE-INSERT-NONFULL(s, k)
            }
            else
            {
                InsertNonFull(r, key);
            }

        }

        /// <summary>
        /// Este es la adaptacion que creo que va a servir
        /// </summary>
        /// <param name="element"></param>
        void Insert(Element<Tkey, Tpointer> element)
        {
            Node<Tkey, Tpointer> r = root; //guardamos root en r porque lo necesitaremos despues

            if(root.elements.Count == 2*minimumDegree - 1) //quiere decir que esta lleno
            {
                Node<Tkey, Tpointer> node_s = new Node<Tkey, Tpointer>();
                root = node_s; //hacemos el nuevo nodo s root
                node_s.elements.Capacity = 0;
                node_s.children[0] = r;  //hacemos el que fue root antes, un hijo de s.

                SplitChild(node_s, 0, r);
                InsertNonFull(node_s, element);
            }
            else
            {
                InsertNonFull(r, element);
            }
        }



        // B-TREE-INSERT-NONFULL(x, k)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_x"></param>
        /// <param name="key"></param>
        void InsertNonFull(Node<Tkey, Tpointer> node_x, Tkey key)
        {
            //int i = node_x.elements.Count;

            //if(node_x.IsLeaf())
            //{
            //    while(i > 0 && key.CompareTo(node_x.elements[i].key) == -1)
            //    {
            //        node_x.elements[i + 1] = node_x.elements[i];
            //        i--;
            //    }
            //    node_x.elements[i+1] = 
            //}
        }

        void InsertNonFull(Node<Tkey, Tpointer> node_x, Element<Tkey, Tpointer> element)
        {
            int i = node_x.elements.Count;

            if (node_x.IsLeaf())
            {
                while (i > 0 && element.key.CompareTo(node_x.elements[i].key) == -1)
                {
                    node_x.elements[i + 1] = node_x.elements[i];
                    i--;
                }
                node_x.elements[i + 1] = element;
                node_x.elements.Capacity++;
            }
            else
            {
                while(i > 0 && element.key.CompareTo(node_x.elements[i].key) == -1)
                {
                    i--;
                }
                i++;

                if(node_x.children[i].elements.Capacity == 2*minimumDegree - 1)
                {
                    SplitChild(node_x, i, node_x.children[i]);
                    if(element.key.CompareTo(node_x.elements[i].key) == 1)
                    {
                        i++;
                    }
                    InsertNonFull(node_x.children[i], element);
                }
            }
        }

    }
}
