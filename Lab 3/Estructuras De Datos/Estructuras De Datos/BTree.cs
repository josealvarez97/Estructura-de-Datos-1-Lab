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




    }
}
