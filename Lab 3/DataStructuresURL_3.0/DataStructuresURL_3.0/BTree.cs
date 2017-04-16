using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataStructuresURL_3._0
{

    class BTree<TKey, TValue> where TKey : IComparable<TKey>, IStringParseable<TKey>
    {
        // Constantes
        const string DEFAULT_SEPARATOR = "00000000000";
        const string SINGLE_SEPARATOR = "|";
        const string BIG_SEPARATOR = "||||";
        const int FIELD_LENGTH = 12; //Contando los pipes |


        // Info en RAM del Arbol
        long root; //Apuntador a root
        long nodeInRam; //Apuntador a nodo en RAM
        long order; //Orden
        long minimumDegreeT; //Minimum Degree
        long numberOfNodes; //Tamano
        long height; //Altura
        string treeDiskPath; //Direccion del archivo
        Node<TKey, TValue> nodeInfo; //Objeto Nodo en RAM


        // Manejador del archivo
        FileStream treeFile;




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
            nodeInfo = new Node<TKey, TValue>();
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
            treeFile.Seek(FIELD_LENGTH * 1 + SINGLE_SEPARATOR.Length, SeekOrigin.Current);
            // nodeInfo.Posicion padre <- actualizar



            // Children
            treeFile.Seek(BIG_SEPARATOR.Length, SeekOrigin.Current); //"Read te mueve"...
            nodeInfo.children.Clear();
            for (int i = 0; i < 2 * minimumDegreeT; i++)
            {
                treeFile.Read(fileInBytes, 0, FIELD_LENGTH);
                long childPointer = long.Parse(ByteStringConverter(fileInBytes));
                nodeInfo.children.Add(childPointer);

                if (i != 2 * minimumDegreeT - 1)
                    treeFile.Seek(SINGLE_SEPARATOR.Length, SeekOrigin.Current);
            }

            // Keys
            treeFile.Seek(BIG_SEPARATOR.Length, SeekOrigin.Current);
            nodeInfo.entries.Clear();
            for (int i = 0; i < 2 * minimumDegreeT - 1; i++)
            {
                treeFile.Read(fileInBytes, 0, FIELD_LENGTH);
                nodeInfo.entries.Add(new Entry<TKey, TValue>());

                //key.ParseToObjectType(ByteStringConverter(fileInBytes));

                TKey key = key.ParseToObjectType("");
                nodeInfo.children.Add(childPointer);

                if (i != 2 * minimumDegreeT - 1)
                    treeFile.Seek(SINGLE_SEPARATOR.Length, SeekOrigin.Current);
            }












        }
        void DiskWrite(long x)
        {
            // POSITION / POINTERS
            WriteInFile(treeFile, x.ToString(DEFAULT_SEPARATOR));
            WriteInFile(treeFile, SINGLE_SEPARATOR);
            // POINTERS TO FATHER'S
            WriteInFile(treeFile, "PosicionPadre");
            WriteInFile(treeFile, BIG_SEPARATOR);
            // POINTERS TO CHILDREN
            for (int i = 0; i < (2 * minimumDegreeT); i++)
            {
                if (!nodeInfo.isLeaf)
                    WriteInFile(treeFile, nodeInfo.children[i].ToString(DEFAULT_SEPARATOR));
                else
                    WriteInFile(treeFile, int.MinValue.ToString());
            }
            WriteInFile(treeFile, BIG_SEPARATOR);
            // KEYS
            for (int i = 0; i < nodeInfo.numberOfKeys; i++)
            {
                WriteInFile(treeFile, nodeInfo.entries[i].key.ToString().ToString(new BTreeFormatProvider()));
            }
            WriteInFile(treeFile, BIG_SEPARATOR);
            // VALUES
            for (int i = 0; i < nodeInfo.numberOfKeys; i++)
            {
                WriteInFile(treeFile, nodeInfo.entries[i].value.ToString().ToString(new BTreeFormatProvider()));
            }
            WriteInFile(treeFile, BIG_SEPARATOR + "\n");
        }


        void WriteInFile(FileStream fileToWrite, string Expression)
        {
            fileToWrite.Write(ByteStringConverter(Expression), 0, Expression.Length);
        }
        void WriteInFile(FileStream fileToWrite, byte[] Expression)
        {

        }



        long CalculateLineSize()
        {
            long lineSize = 0;

            // Sum Pointer / Position
            lineSize += FIELD_LENGTH;
            // SumPipe
            lineSize++;
            // SumFatherPosition
            lineSize += FIELD_LENGTH;
            // Sum pipe
            lineSize++;


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
    }
}
