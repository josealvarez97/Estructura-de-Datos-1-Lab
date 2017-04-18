﻿using System;
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
