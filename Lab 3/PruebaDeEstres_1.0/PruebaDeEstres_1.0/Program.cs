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


            //Guid aGuidObj = Guid.NewGuid();
            //Console.WriteLine(aGuidObj.ToString());


            BTree<Int, Int> tree_1 = new BTree<Int, Int>(4, "C:/Users/jjaa0/Documents/GitHub/Estructura-de-Datos-1-Lab/Lab 3/Arboles/Arbol.txt");
            //BTree<Int, Int> tree_1 = new BTree<Int, Int>(3, "C:/Users/Oscar/Desktop/Laboratorio Estructuras/Estructura-de-Datos-1-Lab/Lab 3/Arboles/Arbol.txt");
            Int intObj = new Int();

            //intObj.value = 12;

            //Entry<Int, Int> entry = new Entry<Int, Int>();
            //entry.key = intObj;
            //entry.value = intObj;


            //tree_1.Insert(entry);

            //intObj.value = 5;
            //tree_1.Insert(entry);


            tree_1.Create();

            for (int i = 0; i < 20; i++)
            {

                tree_1.Insert(new Entry<Int, Int>(i.ToString(), i.ToString()));

            }


            //Console.ReadKey();
        }
    }
}
