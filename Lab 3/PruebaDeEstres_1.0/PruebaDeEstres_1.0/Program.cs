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
            const int datos_arbol = 100000;
            const int datosLista = 1000;
            Console.WriteLine("INGRESE NOMBRE DE ARCHIVO PARA PRESENTAR RESULTADO FINAL");
            string fileName = Console.ReadLine();
            Console.WriteLine("INGRESE DIRECCION DEL DIRECTORIO DONDE DESEA GUARDAR EL ARCHIVO ANTERIOR");
            string directory = Console.ReadLine();/*"C:/Users/Oscar/Desktop/Laboratorio Estructuras/Estructura-de-Datos-1-Lab/Lab 3/Arboles/"*/;/*Console.ReadLine();*/
            Registro objRegistro = new Registro();
            //objRegistro.guidValue = Guid.NewGuid();

            //Registro objRegistro2 = objRegistro;

            using (StreamWriter report = new StreamWriter(directory + fileName))
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
                LinkedList<Registro> list_10 = new LinkedList<Registro>();
                // DECLARACIÓN VARIABLES GLOBALES
                double averageSearchTime = 0;
                TimeSpan average;

                // PROCESO ARBOL<3>
                tree_3.Create();

                for (int i = 0; i < datos_arbol; i++)
                {
                    string GuidValue = Guid.NewGuid().ToString();
                    tree_3.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                    Registro reg1 = new Registro();
                    reg1.guidValue = Guid.Parse(GuidValue);
                    if (i % datosLista == 0)
                        list_3.AddLast(reg1);
                }
                averageSearchTime = 0;
                for (int i = 0; i < list_3.Count; i++)
                {
                    DateTime start = DateTime.Now;
                    tree_3.Search(list_3.ElementAt<Registro>(i));
                    DateTime end = DateTime.Now;

                    averageSearchTime += (end - start).TotalMilliseconds;
                }
                averageSearchTime = averageSearchTime / list_3.Count;
                average = TimeSpan.FromMilliseconds(averageSearchTime);
                report.WriteLine("ARBOL<3> TIEMPO DE INSERCIÓN: " + average.ToString() + "ms");
                // PROCESO ARBOL<4>
                tree_4.Create();

                for (int i = 0; i < datos_arbol; i++)
                {
                    string GuidValue = Guid.NewGuid().ToString();
                    tree_4.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                    Registro reg1 = new Registro();
                    reg1.guidValue = Guid.Parse(GuidValue);
                    if (i % datosLista == 0)
                        list_4.AddLast(reg1);
                }
                averageSearchTime = 0;
                for (int i = 0; i < list_4.Count; i++)
                {
                    DateTime start = DateTime.Now;
                    tree_4.Search(list_4.ElementAt<Registro>(i));
                    DateTime end = DateTime.Now;

                    averageSearchTime += (end - start).TotalMilliseconds;
                }
                averageSearchTime = averageSearchTime / list_4.Count;
                average = TimeSpan.FromMilliseconds(averageSearchTime);
                report.WriteLine("ARBOL<4> TIEMPO DE INSERCIÓN: " + average.ToString() + " ms");
                // PROCESO ARBOL<5>
                tree_5.Create();

                for (int i = 0; i < datos_arbol; i++)
                {
                    string GuidValue = Guid.NewGuid().ToString();
                    tree_5.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                    Registro reg1 = new Registro();
                    reg1.guidValue = Guid.Parse(GuidValue);
                    if (i % datosLista == 0)
                        list_5.AddLast(reg1);
                }
                averageSearchTime = 0;
                for (int i = 0; i < list_5.Count; i++)
                {
                    DateTime start = DateTime.Now;
                    tree_5.Search(list_5.ElementAt<Registro>(i));
                    DateTime end = DateTime.Now;

                    averageSearchTime += (end - start).TotalMilliseconds;
                }
                averageSearchTime = averageSearchTime / list_5.Count;
                average = TimeSpan.FromMilliseconds(averageSearchTime);
                report.WriteLine("ARBOL<5> TIEMPO DE INSERCIÓN: " + average.ToString() + " ms");
                // PROCESO ARBOL<6>
                tree_6.Create();

                for (int i = 0; i < datos_arbol; i++)
                {
                    string GuidValue = Guid.NewGuid().ToString();
                    tree_6.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                    Registro reg1 = new Registro();
                    reg1.guidValue = Guid.Parse(GuidValue);
                    if (i % datosLista == 0)
                        list_6.AddLast(reg1);
                }
                averageSearchTime = 0;
                for (int i = 0; i < list_6.Count; i++)
                {
                    DateTime start = DateTime.Now;
                    tree_6.Search(list_6.ElementAt<Registro>(i));
                    DateTime end = DateTime.Now;

                    averageSearchTime += (end - start).TotalMilliseconds;
                }
                averageSearchTime = averageSearchTime / list_6.Count;
                average = TimeSpan.FromMilliseconds(averageSearchTime);
                report.WriteLine("ARBOL<6> TIEMPO DE INSERCIÓN: " + average.ToString() + " ms");
                //PROCESO ARBOL<7>
                tree_7.Create();

                for (int i = 0; i < datos_arbol; i++)
                {
                    string GuidValue = Guid.NewGuid().ToString();
                    tree_7.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                    Registro reg1 = new Registro();
                    reg1.guidValue = Guid.Parse(GuidValue);
                    if (i % datosLista == 0)
                        list_7.AddLast(reg1);
                }
                averageSearchTime = 0;
                for (int i = 0; i < list_7.Count; i++)
                {
                    DateTime start = DateTime.Now;
                    tree_7.Search(list_7.ElementAt<Registro>(i));
                    DateTime end = DateTime.Now;

                    averageSearchTime += (end - start).TotalMilliseconds;
                }
                averageSearchTime = averageSearchTime / list_7.Count;
                average = TimeSpan.FromMilliseconds(averageSearchTime);
                report.WriteLine("ARBOL<7> TIEMPO DE INSERCIÓN: " + average.ToString() + " ms");
                //PROCESO ARBOL<8>
                tree_8.Create();

                for (int i = 0; i < datos_arbol; i++)
                {
                    string GuidValue = Guid.NewGuid().ToString();
                    tree_8.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                    Registro reg1 = new Registro();
                    reg1.guidValue = Guid.Parse(GuidValue);
                    if (i % datosLista == 0)
                        list_8.AddLast(reg1);
                }
                averageSearchTime = 0;
                for (int i = 0; i < list_8.Count; i++)
                {
                    DateTime start = DateTime.Now;
                    tree_8.Search(list_8.ElementAt<Registro>(i));
                    DateTime end = DateTime.Now;

                    averageSearchTime += (end - start).TotalMilliseconds;
                }
                averageSearchTime = averageSearchTime / list_8.Count;
                average = TimeSpan.FromMilliseconds(averageSearchTime);
                report.WriteLine("ARBOL<8> TIEMPO DE INSERCIÓN: " + average.ToString() + " ms");
                ////PROCESO ARBOL<9>
                //tree_9.Create();

                //for (int i = 0; i < datos_arbol; i++)
                //{
                //    string GuidValue = Guid.NewGuid().ToString();
                //    tree_9.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                //    Registro reg1 = new Registro();
                //    reg1.guidValue = Guid.Parse(GuidValue);
                //    if (i % datosLista == 0)
                //        list_9.AddLast(reg1);
                //}
                //averageSearchTime = 0;
                //for (int i = 0; i < list_9.Count; i++)
                //{
                //    DateTime start = DateTime.Now;
                //    tree_9.Search(list_9.ElementAt<Registro>(i));
                //    DateTime end = DateTime.Now;

                //    averageSearchTime += (end - start).TotalMilliseconds;
                //}
                //averageSearchTime = averageSearchTime / list_9.Count;
                //average = TimeSpan.FromMilliseconds(averageSearchTime);
                //report.WriteLine("ARBOL<9> TIEMPO DE INSERCIÓN: " + average.ToString() + " ms");
                ////PROCESO ARBOL<10>
                //tree_10.Create();

                //for (int i = 0; i < datos_arbol; i++)
                //{
                //    string GuidValue = Guid.NewGuid().ToString();
                //    tree_10.Insert(new Entry<Registro, Registro>(GuidValue, GuidValue));

                //    Registro reg1 = new Registro();
                //    reg1.guidValue = Guid.Parse(GuidValue);
                //    if (i % datosLista == 0)
                //        list_10.AddLast(reg1);
                //}
                //averageSearchTime = 0;
                //for (int i = 0; i < list_10.Count; i++)
                //{
                //    DateTime start = DateTime.Now;
                //    tree_10.Search(list_10.ElementAt<Registro>(i));
                //    DateTime end = DateTime.Now;

                //    averageSearchTime += (end - start).TotalMilliseconds;
                //}
                //averageSearchTime = averageSearchTime / list_10.Count;
                //average = TimeSpan.FromMilliseconds(averageSearchTime);
                //report.WriteLine("ARBOL<10> TIEMPO DE INSERCIÓN: " + average.ToString() + " ms");


            }



        }
    }
}
