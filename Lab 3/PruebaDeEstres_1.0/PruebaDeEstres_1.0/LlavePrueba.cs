using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructuresURL_3._0;

namespace PruebaDeEstres_1._0
{

    class LlavePrueba : IComparable<LlavePrueba>, IStringParseable<LlavePrueba>
    {
        public string DEFAULT_FORMAT_
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DEFAULT_MIN_VAL_FORMAT
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int objectLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IComparable<LlavePrueba>.CompareTo(LlavePrueba other)
        {
            throw new NotImplementedException();
        }

        LlavePrueba IStringParseable<LlavePrueba>.ParseToObjectType(string str)
        {
            throw new NotImplementedException();
        }

        string IStringParseable<LlavePrueba>.ParseToString(LlavePrueba obj)
        {
            throw new NotImplementedException();
        }
    }
}
