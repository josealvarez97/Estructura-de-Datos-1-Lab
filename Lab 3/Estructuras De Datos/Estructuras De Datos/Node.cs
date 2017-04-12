using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estructuras_De_Datos
{
    class Node<Tkey, TPointer> where Tkey : IComparable<Tkey>, IEnumerable<Tkey>
    {
        int minimumDegree;

        //-------------------
        //EVERY NODE HAS THE FOLLOWING FIELDS
        //The number of keys currently stored in node x
        int degree;

        //The number of keys themselves, stored in nondecreasing order
        public List<Element<Tkey, TPointer>> elements;

        //A boolean value that is True if x is a leaf nad False if x is an internal node
        public bool IsLeaf()
        {
            return false;
        }
        public bool IsLeaf(Node<Tkey,TPointer> node_x)
        {
            return false;
        }
        //-----------------


        //Each internal node x also contains 'degree' + 1 pointers to its children. Leaf nodes have no children, so their pointers fields are undefined
        public List<Node<Tkey, TPointer>> children { get; set; }




        /*Every node can contain at most 2t-1 keys. Therefore, an internal node can
         * have at most 2t children. We say that a node is full if it contains exactly
         * 2t-1 keys
         */
        bool IsFull()
        {
            return this.degree == 2 * minimumDegree - 1;
        }



        int Height(int aCurrentHeight = 0)
        {
            if (children.Count != 0)
            {
                aCurrentHeight++;
                foreach (Node<Tkey, TPointer> node in children)
                    node.Height(aCurrentHeight);
            }

            return aCurrentHeight;


        }





        public Element<Tkey, TPointer> Search(Node<Tkey,TPointer> node_x, Tkey keyToSearch)
        {
            int index = 0; // i <= 1


            // Se aumenta mientras el indice sea menor que el grado, ahuevos.
            // && Se aumenta mientras la llave que buscamos sea mayor a la llave en la que estamos...
            //              ...No tendria caso seguir buscando a la "derecha"/aumentando si la llave que buscamos no es mayor a la que estamos.
            while (index <= degree &&  keyToSearch.CompareTo(node_x.elements[index].key) == 1)
                index++;

            // Pues este es el caso trivial. No estoy seguro si la primera condicion es absolutamente necesaria. La segunda si.
            if(index <= degree && keyToSearch.CompareTo(node_x.elements[index].key) == 0)
                return node_x.elements[index];


            // Pues aqui esta la recursion. 
            if(IsLeaf(node_x))
            {
                // Naturalmente si es un nodo hoja no seguimos aplicando recursion.
                return null;
            }
            else
            {
                // Si es un nodo con hijos pues buscamos en los hijos, o mas especificamente, en el hijo que corresponde al indice en el que vamos... 
                //  ...("el que esta a la izquierda del nodo en el que vamos")
                node_x = children[index];
                return Search(node_x, keyToSearch);
            }



            /*
             * B-TREE-SEARCH(x, k)
                    1 i ← 1
                    2 while i ≤ n[x] and k > keyi [x]
                    3   do i ←i + 1
                    4 if i ≤ n[x] and k = keyi [x]
                    5   then return (x, i )
                    6 if leaf [x]
                    7   then return NIL
                    8 else  DISK-READ(ci [x])
                    9       return B-TREE-SEARCH(ci [x], k)
             * 
             * 
             */

        }




        public int Key(Node<Tkey, TPointer> childNode)
        {
            return 0;
        }



    }
}
