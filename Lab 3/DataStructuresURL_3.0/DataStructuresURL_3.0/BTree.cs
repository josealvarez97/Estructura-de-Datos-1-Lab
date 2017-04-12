using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataStructuresURL_3._0
{

    class BTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        long root;
        long nodeInRam;
        int order;
        int minimumDegreeT;
        int numberOfNodes;
        int height;
        const string defaultString = "00000000000";
        const string singleSeparator = "|";
        const string bigSeparator = "||||";
        string treeDiskPath;
        StreamWriter treeFile;

        Node<TKey, TValue> nodeInfo;


        public BTree(int order)
        {
            nodeInfo = new Node<TKey, TValue>();
            treeDiskPath = "~/TreeFolder/BTree.txt";
            treeFile = new StreamWriter(treeDiskPath);
            this.order = order;
            this.minimumDegreeT = order / 2;
            this.numberOfNodes = 0;
            this.height = 0;

            treeFile.WriteLine(int.MinValue);
            treeFile.WriteLine(int.MinValue);
            treeFile.WriteLine(numberOfNodes);
            treeFile.WriteLine(order);
            treeFile.WriteLine(height);


        }


        long AllocateNode()
        {
            return numberOfNodes++;
        }

        void DiskRead(long x)
        {
            int length = 11;
            StreamReader reader = new StreamReader(treeDiskPath);
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();
            reader.ReadLine();

            reader.Read(,)//msdn.microsoft.com/en-us/library/system.io.filestream.seek(v=vs.110).aspx
          





        }
        void DiskWrite(long x)
        {
            // POSITION / POINTERS
            treeFile.Write(x.ToString(defaultString));
            treeFile.Write(singleSeparator);
            // POINTERS TO FATHER'S
            treeFile.Write("PosicionPadre");
            treeFile.Write(bigSeparator);
            // POINTERS TO CHILDREN
            for (int i = 0; i < (2 * minimumDegreeT); i++)
            {
                if (!nodeInfo.isLeaf)
                    treeFile.Write(nodeInfo.children[i].ToString(defaultString));
                else
                    treeFile.Write(int.MinValue);
            }
            treeFile.Write(bigSeparator);
            // KEYS
            for (int i = 0; i < nodeInfo.numberOfKeys; i++)
            {
                treeFile.Write(nodeInfo.entries[i].key.ToString().ToString(new BTreeFormatProvider()));
            }
            treeFile.Write(bigSeparator);
            // VALUES
            for (int i = 0; i < nodeInfo.numberOfKeys; i++)
            {
                treeFile.Write(nodeInfo.entries[i].value.ToString().ToString(new BTreeFormatProvider()));
            }
            treeFile.Write(bigSeparator);
        }
    }
}
