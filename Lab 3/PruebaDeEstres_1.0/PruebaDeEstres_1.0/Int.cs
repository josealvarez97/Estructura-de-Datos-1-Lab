using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructuresURL_3._0;

namespace PruebaDeEstres_1._0
{
    public class Int : IStringParseable<Int>, IComparable<Int>
    {

        public int value { get; set; }

        public Int()
        {
            this.value = 0;
        }

        //https://msdn.microsoft.com/es-es/library/4d7sx9hd(v=vs.110).aspx
        int IComparable<Int>.CompareTo(Int other)
        {
            if (other == null)
                return 1;

            return value.CompareTo(other.value);
        }

        Int IStringParseable<Int>.ParseToObjectType(string str)
        {
            Int intObj = new Int();
            intObj.value = int.Parse(str);

            return intObj;
        }

        string IStringParseable<Int>.ParseToString(Int obj)
        {
            if (obj.value != int.MinValue)
                return obj.value.ToString("00000000000");
            else
                return obj.value.ToString();

        }

    }
}
