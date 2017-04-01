using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estructuras_De_Datos
{
    class Element<Tkey, Tpointer> where Tkey : IComparable<Tkey>,IEnumerable<Tkey>
    {


        public Tkey key { get; set; }

        public Tpointer pointer { get; set; }
    }





    
}
