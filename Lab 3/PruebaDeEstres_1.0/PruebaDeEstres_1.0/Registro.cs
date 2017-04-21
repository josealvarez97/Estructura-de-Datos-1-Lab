using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructuresURL_3._0;

namespace PruebaDeEstres_1._0
{
    class Registro : IStringParseable<Registro>, IComparable<Registro>
    {


        public Guid guidValue { get; set; }

        int IStringParseable<Registro>.objectLength
        {
            get
            {
                return Guid.Empty.ToString().Length;
            }

        }

        public string DEFAULT_FORMAT_
        {
            get
            {
                return "00000000000000000000000000";
            }
        }

        public string DEFAULT_MIN_VAL_FORMAT
        {
            get
            {
                return Guid.Empty.ToString();
            }
        }

        int IComparable<Registro>.CompareTo(Registro other)
        {
            if (other == null)
                return 1;

            return guidValue.CompareTo(other.guidValue);
        }

        Registro IStringParseable<Registro>.ParseToObjectType(string str)
        {
            Registro reg = new Registro();
            reg.guidValue = Guid.Parse(str);
            return reg;
        }

        string IStringParseable<Registro>.ParseToString(Registro obj)
        {

            return obj.guidValue.ToString();
        }
    }
}
