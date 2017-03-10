using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab02_JoseAlvarez_OscarLemus.Extras
{
    public class Node<T> : IEnumerable<T>
    {
        public T Data { get; set; }
        public Node<T> left { get; set; }
        public Node<T> right { get; set; }

        public Node(T data_)
        {
            this.Data = data_;
        }

        public IEnumerator<T> GetEnumerator()
        {

            if (right != null)
            {
                foreach (var v in right)
                {
                    yield return v;
                }
            }

            yield return Data;

            if (left != null)
            {
                foreach (var v in left)
                {
                    yield return v;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        //public void insertData(ref Node<T> node, T data)
        //{
        //    if (node == null)
        //    {
        //        node = new Node<T>(data);

        //    }
        //    else if (node.Data < data)
        //    {
        //        insertData(ref node.right, data);
        //    }

        //    else if (node.Data > data)
        //    {
        //        insertData(ref node.left, data);
        //    }
        //}
    }
}