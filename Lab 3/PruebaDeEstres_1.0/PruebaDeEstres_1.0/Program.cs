using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructuresURL_3._0;
using System.IO;

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

            Console.WriteLine("INGRESE NOMBRE DE ARCHIVO PARA PRESENTAR RESULTADO FINAL");
            string fileName = Console.ReadLine();
            Console.WriteLine("INGRESE DIRECCION DEL DIRECTORIO DONDE DESEA GUARDAR EL ARCHIVO ANTERIOR");
            string directory = "C:/Users/jjaa0/Documents/GitHub/Estructura-de-Datos-1-Lab/Lab 3/Arboles/";/*Console.ReadLine();*/
            using (FileStream report = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {

                // DECLARACIÓN ARBOLES
                BTree<Registro, Registro> tree_3 = new BTree<Registro, Registro>(3, directory + "Arbol(3).btree");
                BTree<Registro, Registro> tree_4 = new BTree<Registro, Registro>(4, directory + "Arbol(4).btree");
                BTree<Registro, Registro> tree_5 = new BTree<Registro, Registro>(5, directory + "Arbol(5).btree");
                BTree<Registro, Registro> tree_6 = new BTree<Registro, Registro>(6, directory + "Arbol(6).btree");
                BTree<Registro, Registro> tree_7 = new BTree<Registro, Registro>(7, directory + "Arbol(7).btree");
                BTree<Registro, Registro> tree_8 = new BTree<Registro, Registro>(8, directory + "Arbol(8).btree");
                BTree<Registro, Registro> tree_9 = new BTree<Registro, Registro>(9, directory + "Arbol(9).btree");
                BTree<Registro, Registro> tree_10 = new BTree<Registro, Registro>(10, directory + "Arbol(10).btree");
                // DECLARACIÓN LISTAS ENLAZADAS
                LinkedList<Registro> list_3 = new LinkedList<Registro>();
                LinkedList<Registro> list_4 = new LinkedList<Registro>();
                LinkedList<Registro> list_5 = new LinkedList<Registro>();
                LinkedList<Registro> list_6 = new LinkedList<Registro>();
                LinkedList<Registro> list_7 = new LinkedList<Registro>();
                LinkedList<Registro> list_8 = new LinkedList<Registro>();
                LinkedList<Registro> list_9 = new LinkedList<Registro>();
                // DECLARACIÓN VARIABLES GLOBALES
                double averageSearchTime = 0;
                TimeSpan average;

                // PROCESO ARBOL<3>
                tree_3.Create();
                for (int i = 0; i < 1000000; i++)
                {
                    string GuidValue = Guid.NewGuid().ToString();
                    tree_3.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                    Registro reg = new Registro();
                    reg.guidValue = Guid.Parse(GuidValue);
                    if (i % 1000 == 0)
                        list_3.AddLast(reg);
                }
                averageSearchTime = 0;
                for (int i = 0; i < 1000; i++)
                {
                    DateTime start = DateTime.Now;
                    tree_3.Search(0, list_3.ElementAt<Registro>(i));
                    DateTime end = DateTime.Now;

                    averageSearchTime += (end - start).TotalSeconds;
                }
                averageSearchTime = averageSearchTime / 1000;
                average = TimeSpan.FromSeconds(averageSearchTime);
                // PROCESO ARBOL<4>
                tree_4.Create();
                for (int i = 0; i < 1000000; i++)
                {
                    string GuidValue = Guid.NewGuid().ToString();
                    tree_4.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                    Registro reg = new Registro();
                    reg.guidValue = Guid.Parse(GuidValue);
                    if (i % 1000 == 0)
                        list_4.AddLast(reg);
                }
                averageSearchTime = 0;
                for (int i = 0; i < 1000; i++)
                {
                    DateTime start = DateTime.Now;
                    tree_4.Search(0, list_4.ElementAt<Registro>(i));
                    DateTime end = DateTime.Now;

                    averageSearchTime += (end - start).TotalSeconds;
                }
                averageSearchTime = averageSearchTime / 1000;
                average = TimeSpan.FromSeconds(averageSearchTime);


            }



        }
    }
}


//List<int> numbers = new List<int>();

//while (numbers.Count <= 20)
//{
//    int number = new Random().Next(0, 101);
//    if (!numbers.Exists(x => x == number))
//        numbers.Add(number);
//}
//for (int i = 0; i <= 20; i++)
//{

//    tree_1.Insert(new Entry<Int, Int>(numbers[i].ToString(), numbers[i].ToString()));

//}


//Console.ReadKey();
//}