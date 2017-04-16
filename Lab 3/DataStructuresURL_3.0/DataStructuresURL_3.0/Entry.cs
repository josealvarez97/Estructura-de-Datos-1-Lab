using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresURL_3._0
{
    class Entry<TKey, TValue> where TKey : IComparable<TKey>, IStringParseable<TKey>
    {

        public TKey key { get; set; }
        public TValue value { get; set; }

        public Entry(TKey key, TValue value, string keystr, string valuestr)
        {
            this.key = key.ParseToObjectType(keystr);
        } 
    }
}
