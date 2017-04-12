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
        StreamWriter treeFile;

        Node<TKey, TValue> nodeInfo;

        
        public BTree(int order)
        {
            nodeInfo = new Node<TKey, TValue>();
            treeFile = new StreamWriter("~/TreeFolder/BTree.txt");
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

        void DiskRead()
        {

        }
        void DiskWrite(long x)
        {
            treeFile.Write(x.ToString(defaultString));
            treeFile.Write(singleSeparator);
            treeFile.Write("PosicionPadre");
            treeFile.Write(bigSeparator);
            for (int i = 0; i < (2 * minimumDegreeT); i++)
            {
                if (!nodeInfo.isLeaf)
                    treeFile.Write(nodeInfo.children[i].ToString(defaultString));
                else
                    treeFile.Write(int.MinValue);
            }
            treeFile.Write(bigSeparator);
            for (int i = 0; i < nodeInfo.numberOfKeys; i++)
            {
                if (!nodeInfo.isLeaf)
                    treeFile.Write(nodeInfo.keys.ToString().ToString(new TKeyFormat()));
                else
                    treeFile.Write(int.MinValue);
            }

        }
    }
}
