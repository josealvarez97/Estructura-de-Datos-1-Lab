using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresURL_3._0
{
    public class Node<TKey, TValue> where TKey : IComparable<TKey>, IStringParseable<TKey> where TValue : IStringParseable<TValue>
    {
        public long numberOfKeys { get; set; }
        public List<Entry<TKey, TValue>> entries { get; set; }
        public bool isLeaf { get; set; }
        public List<long> children { get; set; }
        public int minimumDegreeT { get; set; }

        public Node()
        {
            numberOfKeys = 0;

            entries = new List<Entry<TKey, TValue>>();
            children = new List<long>();
            //minimumDegreeT
        }

        public Node(Node<TKey, TValue> node)
        {
            this.numberOfKeys = node.numberOfKeys;
            TKey keyObj = (TKey) Activator.CreateInstance(typeof(TKey));
            TValue valueObj = (TValue)Activator.CreateInstance(typeof(TValue));


            this.entries = new List<Entry<TKey, TValue>>();
            this.children = new List<long>();

            for (int i = 0; i < node.entries.Count; i++)
            {
                //Entry<TKey, TValue> newEntry = (Entry<TKey, TValue>) Activator.CreateInstance(typeof(Entry<TKey, TValue>), node.entries[i]);
                string keyToAdd = keyObj.ParseToString(node.entries[i].key);
                string valueToAdd = valueObj.ParseToString(node.entries[i].value);
                this.entries.Add(new Entry<TKey, TValue>(keyToAdd, valueToAdd));
            }
            for (int i = 0; i < node.children.Count; i++)
            {
                this.children.Add(node.children[i]);
            }
            this.isLeaf = node.isLeaf;
            this.minimumDegreeT = node.minimumDegreeT;
        }

        public bool IsLeaf()
        {
            bool leaf = true;

            for (int i = 0; i < children.Count; i++)
                if (children[i] != int.MinValue)
                    return (!leaf);

            return leaf;
        }

    }
}
