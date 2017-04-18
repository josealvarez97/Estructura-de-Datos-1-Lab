using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructuresURL_3._0;

namespace PruebaDeEstres_1._0
{

    /*
     * Creating Class Library in Visual C#
     * http://www.c-sharpcorner.com/uploadfile/61b832/creating-class-library-in-visual-c-sharp/
     * Creating C# Class Library (DLL) Using Visual Studio .NET
     * http://www.c-sharpcorner.com/article/creating-C-Sharp-class-library-dll-using-visual-studio-net/
     * Guid.NewGuid Method ()
     * https://msdn.microsoft.com/en-us/library/system.guid.newguid(v=vs.110).aspx
     * 
     * 
     * How to export class libraries in Visual Studio 2012
     * http://stackoverflow.com/questions/19036552/how-to-export-class-libraries-in-visual-studio-2012
     * Access method in class library
     * http://stackoverflow.com/questions/3318602/access-method-in-class-library
     * 
     * */
    class Program
    {
        static void Main(string[] args)
        {


            Guid aGuidObj = Guid.NewGuid();
            Console.WriteLine(aGuidObj.ToString());


            BTree<LlavePrueba, ValorPrueba> tree_1 = new BTree<LlavePrueba, ValorPrueba>(3, "C:/Users/Fed2/Documents/Jose/Github/Estructura-de-Datos-1-Lab/Lab 3/ArbolesCreados.treeFile");
            Console.ReadKey();
        }
    }
}
