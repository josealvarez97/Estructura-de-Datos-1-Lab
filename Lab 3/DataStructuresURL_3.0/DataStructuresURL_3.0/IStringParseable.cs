using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresURL_3._0
{
    public interface IStringParseable<T>
    {

        string ParseToString(T obj);

        T ParseToObjectType(string str);

    }
}
