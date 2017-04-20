using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructuresURL_3._0;

namespace PruebaDeEstres_1._0
{
    class ValorPrueba : IStringParseable<ValorPrueba>
    {
        string IStringParseable<ValorPrueba>.DEFAULT_FORMAT_
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IStringParseable<ValorPrueba>.DEFAULT_MIN_VAL_FORMAT
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IStringParseable<ValorPrueba>.objectLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        ValorPrueba IStringParseable<ValorPrueba>.ParseToObjectType(string str)
        {
            throw new NotImplementedException();
        }

        string IStringParseable<ValorPrueba>.ParseToString(ValorPrueba obj)
        {
            throw new NotImplementedException();
        }
    }
}
