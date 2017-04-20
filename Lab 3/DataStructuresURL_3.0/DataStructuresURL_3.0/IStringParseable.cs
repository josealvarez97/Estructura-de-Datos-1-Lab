using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresURL_3._0
{
    public interface IStringParseable<T>
    {
        int objectLength { get;}
        string DEFAULT_FORMAT_ { get; }
        string DEFAULT_MIN_VAL_FORMAT { get; }

        string ParseToString(T obj);

        T ParseToObjectType(string str);

    }
}
