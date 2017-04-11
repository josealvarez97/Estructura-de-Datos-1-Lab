using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresURL_2._0
{
    class Element<Tkey, TValue> where Tkey : IComparable<Tkey>/*,IEnumerable<Tkey>*/
    {


        public Tkey key { get; set; }

        public TValue value { get; set; }
    }





    
}
