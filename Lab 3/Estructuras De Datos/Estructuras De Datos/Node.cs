using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estructuras_De_Datos
{
    class Node<Tkey, TPointer>
    {
        //-------------------
        //EVERY NODE HAS THE FOLLOWING FIELDS
        //The number of keys currently stored in node x
        int degree;

        //The number of keys themselves, stored in nondecreasing order
        List<Element<Tkey, TPointer>> elements;
        //A boolean value that is True if x is a leaf nad False if x is an internal node
        bool IsLeaf()
        {
            return false;
        }
        //-----------------


        //Each internal node x also contains 'degree' + 1 pointers to its children. Leaf nodes have no children, so their pointers fields are undefined
        List<Node<Tkey, TPointer>> children;



        /*Every node can contain at most 2t-1 keys. Therefore, an internal node can
         * have at most 2t children. We say that a node is full if it contains exactly
         * 2t-1 keys
         */
        bool IsFull()
        {
            return false;
        }













    }
}
