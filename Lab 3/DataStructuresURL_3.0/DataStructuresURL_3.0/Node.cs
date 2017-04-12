using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresURL_3._0
{
    class Node<TKey, TValue> where TKey : IComparable<TKey>
    {
        public long numberOfKeys { get; set; }
        public List<TKey> keys { get; set; }
        public bool isLeaf { get; set; }
        public List<long> children { get; set; }
        public int minimumDegreeT { get; set; }
        
        
        
        
        

    }
}
