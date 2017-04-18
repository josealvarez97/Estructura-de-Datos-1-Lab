using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataStructuresURL_3._0
{

    /*
     * Constraints on Type Parameters (C# Programming Guide)... como usar where...
     * https://msdn.microsoft.com/en-us/library/d5x73970.aspx
     * Introduction to Generics (C# Programming Guide)
     * https://msdn.microsoft.com/en-us/library/0x6a29h6.aspx
     * Generic Methods (C# Programming Guide)
     * https://msdn.microsoft.com/en-us/library/twcad0zb.aspx
     * Generic Interfaces (C# Programming Guide)
     * https://msdn.microsoft.com/en-us/library/kwtft8ak.aspx
     * Generic Type Parameters (C# Programming Guide)
     * https://msdn.microsoft.com/en-us/library/0zk36dx2.aspx
     * 
     */
    public class BTree<TKey, TValue> where TKey : IComparable<TKey>, IStringParseable<TKey> where TValue : IStringParseable<TValue>
    {
        // Constantes
        const string DEFAULT_SEPARATOR = "00000000000";
        const string SINGLE_SEPARATOR = "|";
        const string BIG_SEPARATOR = "||||";
        const int FIELD_LENGTH = 12; //Contando los pipes |


        // Info en RAM del Arbol
        // Invariable
        long order; //Orden
        long minimumDegreeT; //Minimum Degree
        string treeDiskPath; //Direccion del archivo
        // Variable
        long root; //Apuntador a root
        long nodeInRam; //Apuntador a nodo en RAM
        long height; //Altura
        long numberOfNodes; //Tamano


        Node<TKey, TValue> nodeInRamInfo; //Objeto Nodo en RAM


        // Manejador del archivo
        FileStream treeFile;
        //TKey aKeyObj; // (This obj is only used for calling the methods from the IStringParseable<T> interface). Found a better solution... default(Tkey)
        //TValue aValueObj; // (This obj is only used for calling the methods from the IStringParseable<T> interface). Found a better solution... default(TValue)


        public string ByteStringConverter(byte[] bytes)
        {
            return Encoding.UTF32.GetString(bytes);
        }
        public string ByteStringConverter(byte[] bytes, int stringSize)
        {
            return Encoding.UTF32.GetString(bytes).Substring(0, stringSize);
        }
        public byte[] ByteStringConverter(string anString)
        {
            return Encoding.UTF32.GetBytes(anString);
        }


        public BTree(int order)
        {
            nodeInRamInfo = new Node<TKey, TValue>();
            treeDiskPath = "~/TreeFolder/BTree.txt";
            treeFile = File.Create(treeDiskPath);
            this.order = order;
            this.minimumDegreeT = order / 2;
            this.numberOfNodes = 0;
            this.height = 0;

            //Apuntador a Raiz
            WriteInFile(treeFile, int.MinValue.ToString() + "\n");
            //Apuntador a Ultima Posicion Vacia
            WriteInFile(treeFile, int.MinValue.ToString() + "\n");
            //Tamano
            WriteInFile(treeFile, numberOfNodes.ToString() + "\n");
            //Orden
            WriteInFile(treeFile, order.ToString() + "\n");
            //Altura
            WriteInFile(treeFile, height.ToString() + "\n");



        }
        public BTree(int order, string diskPathForTree)
        {
            nodeInRamInfo = new Node<TKey, TValue>();
            treeDiskPath = diskPathForTree;
            treeFile = File.Create(treeDiskPath);
            this.order = order;
            this.minimumDegreeT = order / 2;
            this.numberOfNodes = 0;
            this.height = 0;

            //Apuntador a Raiz
            WriteInFile(treeFile, int.MinValue.ToString() + "\n");
            //Apuntador a Ultima Posicion Vacia
            WriteInFile(treeFile, int.MinValue.ToString() + "\n");
            //Tamano
            WriteInFile(treeFile, numberOfNodes.ToString() + "\n");
            //Orden
            WriteInFile(treeFile, order.ToString() + "\n");
            //Altura
            WriteInFile(treeFile, height.ToString() + "\n");



        }


        long AllocateNode()
        {
            return numberOfNodes++;
        }

        void DiskRead(long x)
        {
            // SE ACTUALIZA EL APUNTADOR DEL NODO EN RAM
            nodeInRam = x;

            byte[] fileInBytes = new byte[FIELD_LENGTH - 2];

            // UPDATE HEADER
            // Apuntador a Raiz
            treeFile.Seek(FIELD_LENGTH * 0, SeekOrigin.Begin);
            treeFile.Read(fileInBytes, 0, FIELD_LENGTH);
            root = long.Parse(ByteStringConverter(fileInBytes));

            // Apuntador a ultima posicion vacia, no se actualiza en RAM
            treeFile.Seek(FIELD_LENGTH * 1, SeekOrigin.Begin);
            //aqui corresponderia actualizar ultima posicion vacia

            // Tamano
            treeFile.Seek(FIELD_LENGTH * 2, SeekOrigin.Begin);

            treeFile.Read(fileInBytes, 0, FIELD_LENGTH);
            numberOfNodes = long.Parse(ByteStringConverter(fileInBytes));

            // Orden
            treeFile.Seek(FIELD_LENGTH * 3, SeekOrigin.Begin);
            treeFile.Read(fileInBytes, 0, FIELD_LENGTH);
            order = long.Parse(ByteStringConverter(fileInBytes));

            // Altura
            treeFile.Seek(FIELD_LENGTH * 4, SeekOrigin.Begin);
            treeFile.Read(fileInBytes, 0, FIELD_LENGTH);
            height = long.Parse(ByteStringConverter(fileInBytes));


            //UPDATE NODE IN RAM
            int headerSize = FIELD_LENGTH * 5;

            long lineSize = CalculateLineSize();

            long PositionToSeekInBytes = headerSize + x * lineSize;
            treeFile.Seek(PositionToSeekInBytes, SeekOrigin.Begin);

            // Father Position
            //treeFile.Seek(FIELD_LENGTH * 1 + SINGLE_SEPARATOR.Length, SeekOrigin.Current);
            // nodeInfo.Posicion padre <- actualizar


            treeFile.Seek(FIELD_LENGTH, SeekOrigin.Current);
            // Children
            treeFile.Seek(BIG_SEPARATOR.Length, SeekOrigin.Current); //"Read te mueve"...
            nodeInRamInfo.children.Clear();
            for (int i = 0; i < (2 * minimumDegreeT); i++)
            {
                treeFile.Read(fileInBytes, 0, FIELD_LENGTH);
                long childPointer = long.Parse(ByteStringConverter(fileInBytes));
                nodeInRamInfo.children.Add(childPointer);

                if (i != (2 * minimumDegreeT) - 1)
                    treeFile.Seek(SINGLE_SEPARATOR.Length, SeekOrigin.Current);
            }

            // Keys
            treeFile.Seek(BIG_SEPARATOR.Length, SeekOrigin.Current);
            nodeInRamInfo.entries.Clear();
            for (int i = 0; i < (2 * minimumDegreeT - 1); i++)
            {
                // Leemos una key
                treeFile.Read(fileInBytes, 0, FIELD_LENGTH);
                //Convertimos la key leida
                TKey keyToAdd = default(TKey);
                keyToAdd = keyToAdd.ParseToObjectType(ByteStringConverter(fileInBytes));


                nodeInRamInfo.entries.Add(new Entry<TKey, TValue>());
                nodeInRamInfo.entries[i].key = keyToAdd;

                if (i != (2 * minimumDegreeT - 1) - 1)
                    treeFile.Seek(SINGLE_SEPARATOR.Length, SeekOrigin.Current);




                /*
                 * Un error que tomo tiempo resolver en esta parte fue el no poder realizar keyToAdd = keyToAdd.ParseToObjectType(ByteStringConverter(fileInBytes));...
                 * 
                 * No entendiamos porque ya que explicitamente se decia arriba que tkey heredaba de la interfaz IStringParseable<tkey> la cual garantizaria que tkey tendria metodos para 
                 * parsearse desde y hacia un string... resulto que en efecto la teoria estaba bien. El clavo, era que no podiamos ejecutar tal linea de codigo sin antes asignar keyToAdd... en 
                 * otras palabras inicializar el objeto, ya que no se puede (o eso creo en este punto de mi vida) declarar metodos estaticos en una interfaz
                 * 
                 * la solucion fue asignar / inicializar keyToAdd = default(TKey), de esta forma ya pudimos llamar los metodos que necesitabamos.
                 * 
                 * 
                 * */
                //TValue valueToAdd = default(TValue);


                //valueToAdd = valueToAdd.ParseToObjectType(ByteStringConverter(fileInBytes));


                //key.ParseToObjectType(ByteStringConverter(fileInBytes));

                //TKey key = key.ParseToObjectType("");
                //nodeInfo.children.Add(childPointer);
            }

            // Values
            treeFile.Seek(BIG_SEPARATOR.Length, SeekOrigin.Current);

            for (int i = 0; i < (2 * minimumDegreeT - 1); i++)
            {
                //Leemos un vlaue
                treeFile.Read(fileInBytes, 0, FIELD_LENGTH);
                //Convertimos el value leido
                TValue valueToAdd = default(TValue);
                valueToAdd = valueToAdd.ParseToObjectType(ByteStringConverter(fileInBytes));

                nodeInRamInfo.entries[i].value = valueToAdd;



                if (i != (2 * minimumDegreeT - 1) - 1) //Para mantener las cosas estandarizadas se dejara esta condicion.. que se hubiera podido obviar no escribir mas codigo para saltar el ultimo pipe de la linea...
                    treeFile.Seek(SINGLE_SEPARATOR.Length, SeekOrigin.Current);
            }



            // Ultimo Pipe de la linea
            //treeFile.Seek(SINGLE_SEPARATOR.Length, SeekOrigin.Current); De hecho creo que no es necesario saltar dicho pipe....

        }


        //string ParseToString(TKey obj)
        //{
        //    return "";
        //}

        //TKey ParseToObjectType(string str)
        //{
        //    return default(TKey);
        //}

        void DiskWrite(long x)
        {
            //El cursor debe estar siempre en la posicion donde se va a escribir...
            int headerSize = FIELD_LENGTH * 5;
            long lineSize = CalculateLineSize();
            long currentFreePosition = headerSize + lineSize * x;
            treeFile.Seek(currentFreePosition, SeekOrigin.Begin);

            // POSITION / POINTERS
            WriteInFile(treeFile, x.ToString(DEFAULT_SEPARATOR));
            WriteInFile(treeFile, SINGLE_SEPARATOR);
            // POINTERS TO FATHER'S
            //WriteInFile(treeFile, "PosicionPadre");
            //WriteInFile(treeFile, BIG_SEPARATOR);
            // POINTERS TO CHILDREN
            for (int i = 0; i < (2 * minimumDegreeT); i++)
            {
                if (!nodeInRamInfo.isLeaf)
                    WriteInFile(treeFile, nodeInRamInfo.children[i].ToString(DEFAULT_SEPARATOR));
                else
                    WriteInFile(treeFile, int.MinValue.ToString());
            }
            WriteInFile(treeFile, BIG_SEPARATOR);
            // KEYS
            for (int i = 0; i < nodeInRamInfo.numberOfKeys; i++)
            {
                WriteInFile(treeFile, nodeInRamInfo.entries[i].key.ToString().ToString(new BTreeFormatProvider()));
            }
            WriteInFile(treeFile, BIG_SEPARATOR);
            // VALUES
            for (int i = 0; i < nodeInRamInfo.numberOfKeys; i++)
            {
                WriteInFile(treeFile, nodeInRamInfo.entries[i].value.ToString().ToString(new BTreeFormatProvider()));
            }
            WriteInFile(treeFile, BIG_SEPARATOR + "\n");
        }


        void DiskWrite(long x, Node<TKey, TValue> nodeInfoToWrite)
        {
            Node<TKey, TValue> pivotNode = nodeInRamInfo;
            nodeInRamInfo = nodeInfoToWrite;
            DiskWrite(x);
        }
        
        void DiskModify(long x)
        {
            // Por fines practicos se utilizara disk write siempre... si no existe se escribira algo nuevo, si se modifico se sobre escribira completo

            ////El cursor debe estar en la posicion donde ya se encuentra escrito x
            //int headerSize = FIELD_LENGTH * 5;
            //long lineSize = CalculateLineSize();
            //long positionToModify = headerSize + lineSize * x;
            //treeFile.Seek(positionToModify, SeekOrigin.Begin);


            ////--------------------------------- reutilizamo parte del codigo de disk-write----------------------------------
            //// POSITION / POINTERS
            //WriteInFile(treeFile, x.ToString(DEFAULT_SEPARATOR));
            //WriteInFile(treeFile, SINGLE_SEPARATOR);
            //// POINTERS TO FATHER'S
            ////WriteInFile(treeFile, "PosicionPadre");
            ////WriteInFile(treeFile, BIG_SEPARATOR);
            //// POINTERS TO CHILDREN
            //for (int i = 0; i < (2 * minimumDegreeT); i++)
            //{
            //    if (!nodeInfo.isLeaf)
            //        WriteInFile(treeFile, nodeInfo.children[i].ToString(DEFAULT_SEPARATOR));
            //    else
            //        WriteInFile(treeFile, int.MinValue.ToString());
            //}
            //WriteInFile(treeFile, BIG_SEPARATOR);
            //// KEYS
            //for (int i = 0; i < nodeInfo.numberOfKeys; i++)
            //{
            //    WriteInFile(treeFile, nodeInfo.entries[i].key.ToString().ToString(new BTreeFormatProvider()));
            //}
            //WriteInFile(treeFile, BIG_SEPARATOR);
            //// VALUES
            //for (int i = 0; i < nodeInfo.numberOfKeys; i++)
            //{
            //    WriteInFile(treeFile, nodeInfo.entries[i].value.ToString().ToString(new BTreeFormatProvider()));
            //}
            //WriteInFile(treeFile, BIG_SEPARATOR + "\n");


        }



        void WriteInFile(FileStream fileToWrite, string Expression)
        {
            fileToWrite.Write(ByteStringConverter(Expression), 0, Expression.Length);
        }
        void WriteInFile(FileStream fileToWrite, byte[] Expression)
        {
            fileToWrite.Write(Expression, 0, ByteStringConverter(Expression).Length);
        }



        long CalculateLineSize()
        {
            long lineSize = 0;

            // Sum Pointer / Position
            lineSize += FIELD_LENGTH;
            // SumPipe
            lineSize++;
            // SumFatherPosition
            //lineSize += FIELD_LENGTH;
            //// Sum pipe
            //lineSize++;


            // Sum 2 pipes
            lineSize += 2;


            // SumChildren
            for (int i = 0; i < 2 * minimumDegreeT; i++)
            {
                lineSize++;
                lineSize += FIELD_LENGTH;
            }
            // Sum Pipe
            lineSize++;


            // Sum 2 pipes
            lineSize += 2;


            // Sum Keys
            for (int i = 0; i < (2 * minimumDegreeT - 1); i++)
            {
                lineSize++;
                lineSize += FIELD_LENGTH;
            }
            //Sum Pipe
            lineSize++;


            // Sum 2 pipes
            lineSize += 2;

            // Sum Values
            for (int i = 0; i < (2 * minimumDegreeT - 1); i++)
            {
                lineSize++;
                lineSize += FIELD_LENGTH;
            }
            //Sum Pipe
            lineSize++;



            return lineSize;
        }




        // ------------------------------------------------------------------------------------------------------------------------
        
        //void BTreeCreate()
        //{

        //}
        
        TKey Search(long x, TKey key)
        {
            long i = 0;
            while(i < nodeInRamInfo.numberOfKeys
                && key.CompareTo(nodeInRamInfo.entries[(int)i].key) == 1)
            {
                i++;
            }
            if(i < nodeInRamInfo.numberOfKeys 
                && key.Equals(nodeInRamInfo.entries[(int)i].key))
            {
                return nodeInRamInfo.entries[(int)i].key;
            }
            if(nodeInRamInfo.isLeaf)
            {
                return default(TKey); //Como hacer que esto sea null...
            }
            else
            {
                DiskRead(nodeInRamInfo.children[(int)i]);
                return Search(nodeInRamInfo.children[(int)i], key);
            }

            
        }



        public void Create()
        {
            nodeInRam = AllocateNode();
            nodeInRamInfo.isLeaf = true;
            nodeInRamInfo.numberOfKeys = 0;
            root = nodeInRam;
            DiskWrite(nodeInRam); // will write a lot of not usefull information, but that's how it is meant to be...
        }


        
        public void Insert(FileStream Tree, TKey key)
        {

        }
        public void Insert(Entry<TKey, TValue> entry)
        {
            long r = root;
            long pivotNode = nodeInRam;
            DiskRead(r);
            Node<TKey, TValue> nodeInfo_r = nodeInRamInfo;
            DiskRead(pivotNode);


            if (nodeInfo_r.numberOfKeys == 2*minimumDegreeT - 1)
            {
                long s = AllocateNode();
                Node<TKey, TValue> nodeInfo_s = new Node<TKey, TValue>();
                root = s;
                nodeInfo_s.isLeaf = false;
                nodeInfo_s.numberOfKeys = 0;
                nodeInfo_s.children[0] = r;
                SplitChild(s, nodeInfo_s, 0, r, nodeInfo_r);
                InsertNonFull(s, nodeInfo_s, entry);
            }
            else
            {
                InsertNonFull(r, nodeInfo_r, entry);
            }
        }

        void InsertNonFull(long x, Node<TKey, TValue> xInfo, Entry<TKey, TValue> entry)
        {

            long i = xInfo.numberOfKeys;
            if(xInfo.isLeaf)
            {
                while(i >= 0 && 
                    entry.key.CompareTo(xInfo.entries[(int)i].key) == -1)
                {
                    xInfo.entries[(int)i + 1] = xInfo.entries[(int)i];
                    i--;
                }
                xInfo.entries[(int)i + 1] = entry;
                xInfo.numberOfKeys++;
                DiskWrite(x, xInfo);
            }
            else
            {
                while(i >= 0
                    && entry.key.CompareTo(xInfo.entries[(int)i].key) == -1)
                {
                    i--;
                }
                i++;
                DiskRead(xInfo.children[(int)i]); // WE READ THE CHILDREN HERE INTO MAIN MEMORY
                if (nodeInRamInfo.numberOfKeys == 2 * minimumDegreeT - 1)
                {
                    SplitChild(x, xInfo, i, xInfo.children[(int)i], nodeInRamInfo);
                    if (entry.key.CompareTo(xInfo.entries[(int)i].key) == 1)
                        i++;
                }
                InsertNonFull(xInfo.children[(int)i], nodeInRamInfo, entry);
            }
        }







        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentNode_X">Assumed to be in main memory</param>
        /// <param name="i"></param>
        /// <param name="childNode_y">Assumed to be in main memory</param>
        public void SplitChild(long parentNode_X, Node<TKey, TValue> nodeInfo_X, long i, long childNode_y, Node<TKey, TValue> nodeInfo_y)
        {
            long z = AllocateNode();
            Node<TKey, TValue> nodeInfo_z = new Node<TKey, TValue>();
            nodeInfo_z.isLeaf = nodeInfo_y.isLeaf;
            nodeInfo_z.numberOfKeys = minimumDegreeT - 1;
            for (long j = 0; j < minimumDegreeT - 1; j++)
            {
                nodeInfo_z.entries[(int)j] = nodeInfo_y.entries[(int)j + (int)minimumDegreeT];
            }
            if(!nodeInfo_y.isLeaf)
            {
                for (long j = 0; j < minimumDegreeT; j++)
                {
                    nodeInfo_z.children[(int)j] = nodeInfo_y.children[(int)j + (int)minimumDegreeT]; 
                }
            }
            nodeInfo_y.numberOfKeys = minimumDegreeT - 1;
            for(long j = nodeInfo_X.numberOfKeys + 1; j == i +1; j--)
            {
                nodeInfo_X.children[(int)j + 1] = nodeInfo_X.children[(int)j];
            }
            nodeInfo_X.children[(int)i + 1] = z;
            for(long j = nodeInfo_X.numberOfKeys; j == i; j--)
            {
                nodeInfo_X.entries[(int)j + 1] = nodeInfo_X.entries[(int)j];
            }
            nodeInfo_X.entries[(int)i] = nodeInfo_y.entries[(int)minimumDegreeT];
            nodeInfo_X.numberOfKeys++;


            DiskWrite(childNode_y, nodeInfo_y);
            DiskWrite(z, nodeInfo_z);
            DiskWrite(parentNode_X, nodeInfo_X);
        }


        //---------------------------------------------------------------------------------------------------------------------------------






    }
}
