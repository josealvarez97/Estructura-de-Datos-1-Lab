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
        const string DEFAULT_FORMAT = "00000000000";
        const string SINGLE_SEPARATOR = "|";
        const string BIG_SEPARATOR = "||||";
        const int FIELD_LENGTH_PIPES = 12; //Contando los pipes |
        const int FIELD_LENGTH_CHARS = 11; //Contando los caracteres


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
        //FileStream treeFile;
        TKey keyInstance; // (This obj is only used for calling the methods from the IStringParseable<T> interface). Found a better solution... default(Tkey)
        TValue valueInstance; // (This obj is only used for calling the methods from the IStringParseable<T> interface). Found a better solution... default(TValue)


        public string ByteStringConverter(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
        public string ByteStringConverter(byte[] bytes, int stringSize)
        {
            return Encoding.ASCII.GetString(bytes).Substring(0, stringSize);
        }
        public byte[] ByteStringConverter(string anString)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(anString);
            return bytes;
        }


        public BTree(int order) //MALO
        {
            //    nodeInRamInfo = new Node<TKey, TValue>();
            //    treeDiskPath = "~/TreeFolder/BTree.txt";
            //    treeFile = File.Create(treeDiskPath);
            //    this.order = order;
            //    this.minimumDegreeT = order / 2;
            //    this.numberOfNodes = 0;
            //    this.height = 0;

            //    //Apuntador a Raiz
            //    WriteInFile(treeFile, int.MinValue.ToString("00000000000") + "\n");
            //    //Apuntador a Ultima Posicion Vacia
            //    WriteInFile(treeFile, int.MinValue.ToString("00000000000") + "\n");
            //    //Tamano
            //    WriteInFile(treeFile, numberOfNodes.ToString("00000000000") + "\n");
            //    //Orden
            //    WriteInFile(treeFile, order.ToString("00000000000") + "\n");
            //    //Altura
            //    WriteInFile(treeFile, height.ToString("00000000000") + "\n");



        }
        public BTree(int order, string diskPathForTree)
        {
            treeDiskPath = diskPathForTree;
            using (FileStream treeFile = new FileStream(treeDiskPath, FileMode.Create, FileAccess.ReadWrite))
            {
                nodeInRamInfo = new Node<TKey, TValue>();

                this.order = order;
                this.minimumDegreeT = order / 2;
                this.numberOfNodes = 0;
                this.height = 0;
                //Estos no salvaron :v con instanciar tkey
                //http://stackoverflow.com/questions/6410340/generics-in-c-sharp-how-can-i-create-an-instance-of-a-variable-type-with-an-ar
                //keyToAdd = (TKey)Activator.CreateInstance(typeof(TKey), new object[] { null, null });

                //http://stackoverflow.com/questions/752/get-a-new-object-instance-from-a-type
                keyInstance = (TKey)Activator.CreateInstance(typeof(TKey)/*, new object()*/);
                valueInstance = (TValue)Activator.CreateInstance(typeof(TValue));

                //Apuntador a Raiz
                WriteInFile(treeFile, int.MinValue.ToString() + "\n");
                //Apuntador a Ultima Posicion Vacia
                WriteInFile(treeFile, int.MinValue.ToString() + "\n");
                //Tamano
                WriteInFile(treeFile, numberOfNodes.ToString(DEFAULT_FORMAT) + "\n");
                //Orden
                WriteInFile(treeFile, order.ToString(DEFAULT_FORMAT) + "\n");
                //Altura
                WriteInFile(treeFile, height.ToString(DEFAULT_FORMAT) + "\n");



            }


        }


        long AllocateNode()
        {
            long memoryPositionForNewNode = numberOfNodes;
            Node<TKey, TValue> dummyNode = new Node<TKey, TValue>();
            dummyNode.isLeaf = true;
            DiskWrite(memoryPositionForNewNode, dummyNode);
            return numberOfNodes++;
        }

        void DiskRead(long x)
        {
            using (FileStream treeFile = new FileStream(treeDiskPath, FileMode.Open, FileAccess.ReadWrite))
            {
                /*
 * quiero saltar con begin... quiero saltar pipes... hay 12 pipes en cada field de encabezado ¿no? (quien lea esto cuente :v)
 * 
 * pero..... si se va a mover con current... piense que son cantidad de desplazamientos a la derecha. quiere moverse 11 caracteres, no use la constante 
 * 
 * de pipes porque esa es 12! Use la de chars... y eso.
 * 
 * al final la de pipes solo sirvio para calculos en el encabezado...
 */

                // SE ACTUALIZA EL APUNTADOR DEL NODO EN RAM
                nodeInRam = x;

                byte[] fileInBytes = new byte[FIELD_LENGTH_CHARS];
                byte[] fileInBytes_keySize = new byte[keyInstance.objectLength];
                byte[] fileInBytes_valueSize = new byte[valueInstance.objectLength];

                // UPDATE HEADER
                // Apuntador a Raiz
                treeFile.Seek(FIELD_LENGTH_PIPES * 0, SeekOrigin.Begin);
                treeFile.Read(fileInBytes, 0, FIELD_LENGTH_CHARS);
                root = long.Parse(ByteStringConverter(fileInBytes));

                // Apuntador a ultima posicion vacia, no se actualiza en RAM
                treeFile.Seek(FIELD_LENGTH_PIPES * 1, SeekOrigin.Begin);
                //aqui corresponderia actualizar ultima posicion vacia

                // Tamano
                treeFile.Seek(FIELD_LENGTH_PIPES * 2, SeekOrigin.Begin);

                treeFile.Read(fileInBytes, 0, FIELD_LENGTH_CHARS);
                numberOfNodes = long.Parse(ByteStringConverter(fileInBytes));

                // Orden
                treeFile.Seek(FIELD_LENGTH_PIPES * 3, SeekOrigin.Begin);
                treeFile.Read(fileInBytes, 0, FIELD_LENGTH_CHARS);
                order = long.Parse(ByteStringConverter(fileInBytes));

                // Altura
                treeFile.Seek(FIELD_LENGTH_PIPES * 4, SeekOrigin.Begin);
                treeFile.Read(fileInBytes, 0, FIELD_LENGTH_CHARS);
                height = long.Parse(ByteStringConverter(fileInBytes));

                UpdateFileHeader(treeFile);

                //UPDATE NODE IN RAM
                int headerSize = FIELD_LENGTH_PIPES * 5;

                long lineSize = CalculateLineSize();

                long PositionToSeekInBytes = headerSize + x * lineSize;
                treeFile.Seek(PositionToSeekInBytes, SeekOrigin.Begin);

                // Father Position
                //treeFile.Seek(FIELD_LENGTH * 1 + SINGLE_SEPARATOR.Length, SeekOrigin.Current);
                // nodeInfo.Posicion padre <- actualizar


                treeFile.Seek(FIELD_LENGTH_CHARS, SeekOrigin.Current);
                // Children
                treeFile.Seek(BIG_SEPARATOR.Length, SeekOrigin.Current); //"Read te mueve"...
                nodeInRamInfo.children.Clear();
                for (int i = 0; i < (order); i++)
                {
                    treeFile.Read(fileInBytes, 0, FIELD_LENGTH_CHARS);
                    long childPointer = long.Parse(ByteStringConverter(fileInBytes));
                    nodeInRamInfo.children.Add(childPointer);

                    if (i != (order) - 1)
                        treeFile.Seek(SINGLE_SEPARATOR.Length, SeekOrigin.Current);
                }
                nodeInRamInfo.isLeaf = nodeInRamInfo.IsLeaf();

                // Keys
                treeFile.Seek(BIG_SEPARATOR.Length, SeekOrigin.Current);
                nodeInRamInfo.entries.Clear();
                nodeInRamInfo.numberOfKeys = 0;
                for (int i = 0; i < (order - 1); i++)
                {
                    // Leemos una key
                    treeFile.Read(fileInBytes_keySize, 0, keyInstance.objectLength);

                    if (ByteStringConverter(fileInBytes_keySize) != keyInstance.DEFAULT_MIN_VAL_FORMAT)
                        nodeInRamInfo.numberOfKeys++;
                    //Convertimos la key leida


                    keyInstance = keyInstance.ParseToObjectType(ByteStringConverter(fileInBytes_keySize));


                    nodeInRamInfo.entries.Add(new Entry<TKey, TValue>());
                    nodeInRamInfo.entries[i].key = keyInstance;


                    if (i != (order - 1) - 1)
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

                for (int i = 0; i < (order - 1); i++)
                {
                    //Leemos un vlaue
                    treeFile.Read(fileInBytes_valueSize, 0, valueInstance.objectLength);
                    //Convertimos el value leido
                    //TValue valueToAdd = default(TValue);
                    valueInstance = valueInstance.ParseToObjectType(ByteStringConverter(fileInBytes_valueSize));

                    nodeInRamInfo.entries[i].value = valueInstance;



                    if (i != (order - 1) - 1) //Para mantener las cosas estandarizadas se dejara esta condicion.. que se hubiera podido obviar no escribir mas codigo para saltar el ultimo pipe de la linea...
                        treeFile.Seek(SINGLE_SEPARATOR.Length, SeekOrigin.Current);
                }



                // Ultimo Pipe de la linea
                //treeFile.Seek(SINGLE_SEPARATOR.Length, SeekOrigin.Current); De hecho creo que no es necesario saltar dicho pipe....

            }


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
            using (FileStream treeFile = new FileStream(treeDiskPath, FileMode.Open, FileAccess.ReadWrite))
            {
                //treeFile = File.Open(treeDiskPath, FileMode.Open);
                //El cursor debe estar siempre en la posicion donde se va a escribir...
                int headerSize = FIELD_LENGTH_PIPES * 5;
                long lineSize = CalculateLineSize();
                long currentFreePosition = headerSize + lineSize * x;
                treeFile.Seek(currentFreePosition, SeekOrigin.Begin);

                // POSITION / POINTERS
                WriteInFile(treeFile, x.ToString(DEFAULT_FORMAT));
                //WriteInFile(treeFile, SINGLE_SEPARATOR);
                // POINTERS TO FATHER'S
                //WriteInFile(treeFile, "PosicionPadre");
                WriteInFile(treeFile, BIG_SEPARATOR);
                // POINTERS TO CHILDREN
                for (int i = 0; i < order; i++)
                {
                    if (!nodeInRamInfo.isLeaf
                        && nodeInRamInfo.children[i] != int.MinValue)
                        WriteInFile(treeFile, nodeInRamInfo.children[i].ToString(DEFAULT_FORMAT));
                    else
                        WriteInFile(treeFile, int.MinValue.ToString());

                    if (i != (order) - 1)
                        WriteInFile(treeFile, SINGLE_SEPARATOR);

                }
                WriteInFile(treeFile, BIG_SEPARATOR);
                // KEYS
                for (int i = 0; i < order - 1; i++)
                {
                    if (i < nodeInRamInfo.numberOfKeys)
                        WriteInFile(treeFile, keyInstance.ParseToString(nodeInRamInfo.entries[i].key));
                    else
                        WriteInFile(treeFile, keyInstance.DEFAULT_MIN_VAL_FORMAT);


                    if (i != (order - 1) - 1)
                        WriteInFile(treeFile, SINGLE_SEPARATOR);
                }
                WriteInFile(treeFile, BIG_SEPARATOR);
                // VALUES
                for (int i = 0; i < order - 1; i++)
                {
                    if (i < nodeInRamInfo.numberOfKeys)
                        WriteInFile(treeFile, valueInstance.ParseToString(nodeInRamInfo.entries[i].value));
                    else
                        WriteInFile(treeFile, valueInstance.DEFAULT_MIN_VAL_FORMAT);

                    if (i != (order - 1) - 1)
                        WriteInFile(treeFile, SINGLE_SEPARATOR);
                }
                WriteInFile(treeFile, "\n");

            }


        }


        void DiskWrite(long x, Node<TKey, TValue> nodeInfoToWrite)
        {
            Node<TKey, TValue> pivotNode = nodeInRamInfo;
            nodeInRamInfo = nodeInfoToWrite;
            DiskWrite(x);
            nodeInRamInfo = pivotNode;
        }





        void WriteInFile(FileStream fileToWrite, string Expression)
        {
            fileToWrite.Write(ByteStringConverter(Expression), 0, ByteStringConverter(Expression).Length);
        }
        void WriteInFile(FileStream fileToWrite, byte[] Expression)
        {
            fileToWrite.Write(Expression, 0, Expression.Length);
        }



        long CalculateLineSize()
        {
            long lineSize = 0;
            // (SUMAREMOS CARACTERES)
            // Sum Pointer / Position
            lineSize += FIELD_LENGTH_CHARS;
            // Sum 4 pipes
            lineSize += BIG_SEPARATOR.Length;
            //// SumChildren
            for (int i = 0; i < order; i++)
            {
                lineSize += FIELD_LENGTH_CHARS;
                if (i != (2 * minimumDegreeT) - 1)
                    lineSize++;
            }
            // Sum 4 pipes
            lineSize += BIG_SEPARATOR.Length;
            // Sum Keys
            TKey keyObj = (TKey)Activator.CreateInstance(typeof(TKey));
            for (int i = 0; i < order - 1; i++)
            {
                lineSize += keyObj.objectLength;
                if (i != (order - 1) - 1)
                    lineSize++;
            }
            // Sum 4 pipes
            lineSize += BIG_SEPARATOR.Length;
            // Sum Values
            TValue valueObj = (TValue)Activator.CreateInstance(typeof(TValue));
            for (int i = 0; i < order - 1; i++)
            {
                lineSize += valueObj.objectLength;
                if (i != (order - 1) - 1)
                    lineSize++;
            }


            return lineSize + 1 /* + salto de linea*/;
        }






        // ------------------------------------------------------------------------------------------------------------------------

        //void BTreeCreate()
        //{

        //}

        public TKey Search(long x, TKey key)
        {
            long i = 0;
            while (i < nodeInRamInfo.numberOfKeys
                && key.CompareTo(nodeInRamInfo.entries[(int)i].key) == 1)
            {
                i++;
            }
            if (i < nodeInRamInfo.numberOfKeys
                && key.Equals(nodeInRamInfo.entries[(int)i].key))
            {
                return nodeInRamInfo.entries[(int)i].key;
            }
            if (nodeInRamInfo.isLeaf)
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
            UpdateFileHeader();
        }


        public void Insert(Entry<TKey, TValue> entry)
        {
            long r = root;
            long pivotNode = nodeInRam;
            DiskRead(r);

            Node<TKey, TValue> nodeInfo_r = new Node<TKey, TValue>(nodeInRamInfo);
            DiskRead(pivotNode);


            if (nodeInfo_r.numberOfKeys == (2 * minimumDegreeT) - 1)
            {
                long s = AllocateNode();
                Node<TKey, TValue> nodeInfo_s = new Node<TKey, TValue>();
                root = s;
                UpdateFileHeader();
                nodeInfo_s.isLeaf = false;
                nodeInfo_s.numberOfKeys = 0;
                nodeInfo_s.children.Add(r);//nodeInfo_s.children[0] = r;
                SplitChild(s, nodeInfo_s, 0, r, nodeInfo_r);
                InsertNonFull(s, nodeInfo_s, entry);
            }
            else//9--------------
            {
                InsertNonFull(r, nodeInfo_r, entry);
            }
            UpdateFileHeader();
        }

        void InsertNonFull(long x, Node<TKey, TValue> xInfo, Entry<TKey, TValue> entry)
        {

            long i = xInfo.numberOfKeys - 1;
            if (xInfo.isLeaf)
            {
                while (i >= 0)
                {
                    if (entry.key.CompareTo(xInfo.entries[(int)(i)].key) == -1)
                    {
                        xInfo.entries[(int)(i) + 1] = xInfo.entries[(int)i];
                        i--;
                    }
                    else
                    {
                        break;
                    }

                }
                xInfo.entries[(int)(i) + 1] = entry;
                xInfo.numberOfKeys++;
                DiskWrite(x, xInfo);
            }
            else
            {
                while (i >= 0)
                {
                    if (entry.key.CompareTo(xInfo.entries[(int)(i)].key) == -1)
                        i--;
                    else
                        break;
                }
                i++;
                DiskRead(xInfo.children[(int)(i)]); // WE READ THE CHILDREN HERE INTO MAIN MEMORY
                if (nodeInRamInfo.numberOfKeys == 2 * minimumDegreeT - 1)
                {
                    SplitChild(x, xInfo, i, xInfo.children[(int)(i)], nodeInRamInfo);
                    if (entry.key.CompareTo(xInfo.entries[(int)(i)].key) == 1)
                    {
                        i++;
                        DiskRead(xInfo.children[(int)(i)]);
                    }
                }
                InsertNonFull(xInfo.children[(int)(i)], new Node<TKey, TValue>(nodeInRamInfo)/*Una nueva instancia para que DiskRead no se cague en el parametro al modificar nodeInRam*/, entry);
            }
        }


        public List<Entry<TKey, TValue>> expandList(List<Entry<TKey, TValue>> entriesList, long size)
        {
            for (int i = 0; entriesList.Count < size; i++)
            {
                entriesList.Add(new Entry<TKey, TValue>());
            }
            return entriesList;
        }
        public List<long> expandList(List<long> childrenList, long size)
        {
            for (int i = 0; childrenList.Count < size; i++)
            {
                childrenList.Add(int.MinValue);
            }
            return childrenList;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentNode_X">Assumed to be in main memory</param>
        /// <param name="i"></param>
        /// <param name="childNode_y">Assumed to be in main memory</param>
        public void SplitChild(long parentNode_X, Node<TKey, TValue> nodeInfo_X, long i, long childNode_y, Node<TKey, TValue> nodeInfo_y)
        {

            /*
             *          X | | | | 
             *           /  \
             *          /    \
             *     Y |||||     Z||||||
             * 
             */
            long z = AllocateNode();
            UpdateFileHeader();
            Node<TKey, TValue> nodeInfo_z = new Node<TKey, TValue>();
            nodeInfo_z.isLeaf = nodeInfo_y.isLeaf;
            nodeInfo_z.numberOfKeys = minimumDegreeT - 1;

            //------
            nodeInfo_z.entries = expandList(nodeInfo_z.entries, order - 1);
            nodeInfo_z.children = expandList(nodeInfo_z.children, order);
            nodeInfo_X.entries = expandList(nodeInfo_X.entries, order - 1);
            nodeInfo_X.children = expandList(nodeInfo_X.children, order);

            //--------

            // LE PASAMOS ELEMENTOS DE Y A Z.
            for (long j = 0; j < minimumDegreeT - 1; j++)
            {
                //nodeInfo_z.entries.Add(new Entry<TKey, TValue>());//truco sucio...ntt no es sucio, es shuquisimo
                nodeInfo_z.entries[(int)j] = nodeInfo_y.entries[(int)j + (int)minimumDegreeT];
            }
            if (!nodeInfo_y.isLeaf)
            {
                for (long j = 0; j < minimumDegreeT; j++)
                {
                    nodeInfo_z.children[(int)j] = nodeInfo_y.children[(int)j + (int)minimumDegreeT];
                    nodeInfo_y.children[(int)j + (int)minimumDegreeT] = int.MinValue;
                }
            }
            // 
            nodeInfo_y.numberOfKeys = minimumDegreeT - 1;
            // MOVEMOS HIJOS EN X PARA HACER ESPACIO Y METERLE Z COMO HIJO
            for (long j = nodeInfo_X.numberOfKeys /*+ 1 justamente viene siendo en el arreglo la posicion que queremos puesto numeracion c#*/; j > i; j--)
            {
                nodeInfo_X.children[(int)j + 1] = nodeInfo_X.children[(int)j];

            }
            // METEMOS Z
            nodeInfo_X.children[(int)i + 1] = z;

            // MOVEMOS ELEMENTOS DE X... PARA SUBIR UN ELEMENTO DE Y
            for (long j = nodeInfo_X.numberOfKeys - 1; j > i - 1; j--)
            {
                nodeInfo_X.entries[(int)j + 1] = nodeInfo_X.entries[(int)j];
            }
            // METER ELEMENTO DE Y
            nodeInfo_X.entries[(int)i] = nodeInfo_y.entries[(int)minimumDegreeT /*restamos para ajustar contadores c#*/ - 1];
            nodeInfo_X.numberOfKeys++;


            DiskWrite(childNode_y, nodeInfo_y);
            DiskWrite(z, nodeInfo_z);
            DiskWrite(parentNode_X, nodeInfo_X);
        }


        //---------------------------------------------------------------------------------------------------------------------------------




        void UpdateFileHeader()
        {
            using (FileStream treeFile = new FileStream(treeDiskPath, FileMode.Open, FileAccess.ReadWrite))
            {
                // UPDATE HEADER EN FILE
                // Apuntador a Raiz
                //treeFile = File.Open(treeDiskPath, FileMode.Open);
                treeFile.Seek(FIELD_LENGTH_PIPES * 0, SeekOrigin.Begin);
                WriteInFile(treeFile, root.ToString(DEFAULT_FORMAT));


                // Apuntador a ultima posicion vacia, no se actualiza en RAM
                treeFile.Seek(FIELD_LENGTH_PIPES * 1, SeekOrigin.Begin);
                //aqui corresponderia actualizar ultima posicion vacia
                WriteInFile(treeFile, numberOfNodes.ToString(DEFAULT_FORMAT));
                // Tamano
                treeFile.Seek(FIELD_LENGTH_PIPES * 2, SeekOrigin.Begin);



                WriteInFile(treeFile, numberOfNodes.ToString(DEFAULT_FORMAT));


                // Orden
                treeFile.Seek(FIELD_LENGTH_PIPES * 3, SeekOrigin.Begin);
                WriteInFile(treeFile, order.ToString(DEFAULT_FORMAT));


                // Altura
                treeFile.Seek(FIELD_LENGTH_PIPES * 4, SeekOrigin.Begin);
                WriteInFile(treeFile, height.ToString(DEFAULT_FORMAT));


            }

        }


        void UpdateFileHeader(FileStream treeFile)
        {

            // UPDATE HEADER EN FILE
            // Apuntador a Raiz
            //treeFile = File.Open(treeDiskPath, FileMode.Open);
            treeFile.Seek(FIELD_LENGTH_PIPES * 0, SeekOrigin.Begin);
            WriteInFile(treeFile, root.ToString(DEFAULT_FORMAT));


            // Apuntador a ultima posicion vacia, no se actualiza en RAM
            treeFile.Seek(FIELD_LENGTH_PIPES * 1, SeekOrigin.Begin);
            //aqui corresponderia actualizar ultima posicion vacia
            WriteInFile(treeFile, numberOfNodes.ToString(DEFAULT_FORMAT));
            // Tamano
            treeFile.Seek(FIELD_LENGTH_PIPES * 2, SeekOrigin.Begin);



            WriteInFile(treeFile, numberOfNodes.ToString(DEFAULT_FORMAT));


            // Orden
            treeFile.Seek(FIELD_LENGTH_PIPES * 3, SeekOrigin.Begin);
            WriteInFile(treeFile, order.ToString(DEFAULT_FORMAT));


            // Altura
            treeFile.Seek(FIELD_LENGTH_PIPES * 4, SeekOrigin.Begin);
            WriteInFile(treeFile, height.ToString(DEFAULT_FORMAT));



        }

    }
}
