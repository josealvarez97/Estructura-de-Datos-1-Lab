using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresURL_3._0
{
    public class Entry<TKey, TValue> where TKey : IComparable<TKey>, IStringParseable<TKey> where TValue : IStringParseable<TValue>
    {

        public TKey key { get; set; }
        public TValue value { get; set; }




        public Entry()
        {

        }
        //Los siguientes metodos no se usaran pero seria interesante utilizarlos...
        public Entry(string strTkey, string strTvalue)
        {
            TKey keyToAdd = default(TKey);
            TValue valueToAdd = default(TValue);

            keyToAdd = keyToAdd.ParseToObjectType(strTkey);
            valueToAdd = valueToAdd.ParseToObjectType(strTvalue);

            key = keyToAdd;
            value = valueToAdd;

        } 
    }
}
