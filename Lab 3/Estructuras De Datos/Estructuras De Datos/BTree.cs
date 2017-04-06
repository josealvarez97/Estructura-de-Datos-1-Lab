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
        /// 
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
            for (int j = 0; j < minimumDegree - 1; j++) // for j <- 1 to t-1
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


        }



    }
}
