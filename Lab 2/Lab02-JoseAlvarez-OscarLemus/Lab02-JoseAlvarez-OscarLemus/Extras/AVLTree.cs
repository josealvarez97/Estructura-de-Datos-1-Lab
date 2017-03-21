using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab02_JoseAlvarez_OscarLemus.Extras
{
    public class AVLTree<TKey, TValue> : IEnumerable<TValue>
    {

        private Comparison<TKey> _comparer;
        private AvlNode _root;
        int elementCount;
        public class AvlNode
        {
            public AvlNode Parent;
            public AvlNode Left;
            public AvlNode Right;
            public TKey Key;
            public TValue Value;
            public int Balance;
        }


        //public AVLTree(IComparer<TKey> comparer)
        //{
        //    _comparer = comparer;
        //    elementCount = 0;
        //}

        public AVLTree(Comparison<TKey> comparison)
        {
            _comparer = comparison;

            elementCount = 0;
        }

      //  public AVLTree()
      //: this(Comparer<TKey>.Default)
      //  {

      //  }


        public void Insert(TKey key, TValue value)
        {
            if (_root == null)
            {
                _root = new AvlNode { Key = key, Value = value };
                elementCount++;
            }
            else
            {
                AvlNode node = _root;

                while (node != null)
                {
                    int compare = _comparer(key, node.Key);

                    if (compare < 0)
                    {
                        AvlNode left = node.Left;

                        if (left == null)
                        {
                            node.Left = new AvlNode { Key = key, Value = value, Parent = node };
                            elementCount++;

                            InsertBalance(node, 1);

                            return;
                        }
                        else
                        {
                            node = left;
                        }
                    }
                    else if (compare > 0)
                    {
                        AvlNode right = node.Right;

                        if (right == null)
                        {
                            node.Right = new AvlNode { Key = key, Value = value, Parent = node };
                            elementCount++;

                            InsertBalance(node, -1);

                            return;
                        }
                        else
                        {
                            node = right;
                        }
                    }
                    else
                    {
                        node.Value = value;

                        return;
                    }
                }
            }
        }

        private void InsertBalance(AvlNode node, int balance)
        {
            while (node != null)
            {
                balance = (node.Balance += balance);

                if (balance == 0)
                {
                    return;
                }
                else if (balance == 2)
                {
                    if (node.Left.Balance == 1)
                    {
                        RotateRight(node);
                    }
                    else
                    {
                        RotateLeftRight(node);
                    }

                    return;
                }
                else if (balance == -2)
                {
                    if (node.Right.Balance == -1)
                    {
                        RotateLeft(node);
                    }
                    else
                    {
                        RotateRightLeft(node);
                    }

                    return;
                }

                AvlNode parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.Left == node ? 1 : -1;
                }

                node = parent;
            }
        }

        private AvlNode RotateLeft(AvlNode node)
        {
            AvlNode right = node.Right;
            AvlNode rightLeft = right.Left;
            AvlNode parent = node.Parent;

            right.Parent = parent;
            right.Left = node;
            node.Right = rightLeft;
            node.Parent = right;

            if (rightLeft != null)
            {
                rightLeft.Parent = node;
            }

            if (node == _root)
            {
                _root = right;
            }
            else if (parent.Right == node)
            {
                parent.Right = right;
            }
            else
            {
                parent.Left = right;
            }

            right.Balance++;
            node.Balance = -right.Balance;

            return right;
        }

        private AvlNode RotateRight(AvlNode node)
        {
            AvlNode left = node.Left;
            AvlNode leftRight = left.Right;
            AvlNode parent = node.Parent;

            left.Parent = parent;
            left.Right = node;
            node.Left = leftRight;
            node.Parent = left;

            if (leftRight != null)
            {
                leftRight.Parent = node;
            }

            if (node == _root)
            {
                _root = left;
            }
            else if (parent.Left == node)
            {
                parent.Left = left;
            }
            else
            {
                parent.Right = left;
            }

            left.Balance--;
            node.Balance = -left.Balance;

            return left;
        }


        private AvlNode RotateLeftRight(AvlNode node)
        {
            AvlNode left = node.Left;
            AvlNode leftRight = left.Right;
            AvlNode parent = node.Parent;
            AvlNode leftRightRight = leftRight.Right;
            AvlNode leftRightLeft = leftRight.Left;

            leftRight.Parent = parent;
            node.Left = leftRightRight;
            left.Right = leftRightLeft;
            leftRight.Left = left;
            leftRight.Right = node;
            left.Parent = leftRight;
            node.Parent = leftRight;

            if (leftRightRight != null)
            {
                leftRightRight.Parent = node;
            }

            if (leftRightLeft != null)
            {
                leftRightLeft.Parent = left;
            }

            if (node == _root)
            {
                _root = leftRight;
            }
            else if (parent.Left == node)
            {
                parent.Left = leftRight;
            }
            else
            {
                parent.Right = leftRight;
            }

            if (leftRight.Balance == -1)
            {
                node.Balance = 0;
                left.Balance = 1;
            }
            else if (leftRight.Balance == 0)
            {
                node.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                node.Balance = -1;
                left.Balance = 0;
            }

            leftRight.Balance = 0;

            return leftRight;
        }


        private AvlNode RotateRightLeft(AvlNode node)
        {
            AvlNode right = node.Right;
            AvlNode rightLeft = right.Left;
            AvlNode parent = node.Parent;
            AvlNode rightLeftLeft = rightLeft.Left;
            AvlNode rightLeftRight = rightLeft.Right;

            rightLeft.Parent = parent;
            node.Right = rightLeftLeft;
            right.Left = rightLeftRight;
            rightLeft.Right = right;
            rightLeft.Left = node;
            right.Parent = rightLeft;
            node.Parent = rightLeft;

            if (rightLeftLeft != null)
            {
                rightLeftLeft.Parent = node;
            }

            if (rightLeftRight != null)
            {
                rightLeftRight.Parent = right;
            }

            if (node == _root)
            {
                _root = rightLeft;
            }
            else if (parent.Right == node)
            {
                parent.Right = rightLeft;
            }
            else
            {
                parent.Left = rightLeft;
            }

            if (rightLeft.Balance == 1)
            {
                node.Balance = 0;
                right.Balance = -1;
            }
            else if (rightLeft.Balance == 0)
            {
                node.Balance = 0;
                right.Balance = 0;
            }
            else
            {
                node.Balance = 1;
                right.Balance = 0;
            }

            rightLeft.Balance = 0;

            return rightLeft;
        }

        public int Size() { return elementCount; }


        public TValue SearchOnly(AvlNode node, Comparison<TKey> comparer, TKey key)
        {
            TValue Data = node.Value;
            if (comparer(key, node.Key) == 0)
            {
                return node.Value;
            }
            else if (comparer(key, node.Key) == -1)
            {
                Data = SearchOnly(node.Left, comparer, key);
            }
            else if (comparer(key, node.Key) == 1)
            {
                Data = SearchOnly(node.Right, comparer, key);
            }
            return Data;
        }

        public TValue SearchOnly(Comparison<TKey> comparer, TKey key)
        {
            return SearchOnly(_root, comparer, key);
        }


        /// <summary>
        /// Metodo para buscar en el arbol
        /// </summary>
        /// <param name="node"></param>
        /// <param name="comparer">Criterio para buscar en el arbol, en este caso es de la misma data pero al llamar el metodo se especifica porque caracteristica de la data se desea buscar</param>
        /// <param name="data_">Informacion nueva para colocar</param>
        public void Search(AvlNode node, Comparison<TKey> comparer, TKey key, TValue value)
        {
            if (comparer(key, node.Key) == 0)
            {
                node.Key = key;
                node.Value = value;
            }
            else if (comparer(key, node.Key) == -1)
            {
                Search(node.Left, comparer, key, value);
            }
            else if (comparer(key, node.Key) == 1)
            {
                Search(node.Right, comparer, key, value);
            }
        }

        public void Search(Comparison<TKey> comparer, TKey key, TValue value)
        {
            Search(_root, comparer, key, value);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return new AvlNodeEnumerator(_root);
        }


        sealed class AvlNodeEnumerator : IEnumerator<TValue>
        {
            private AvlNode _root;
            private Action _action;
            private AvlNode _current;
            private AvlNode _right;

            public AvlNodeEnumerator(AvlNode root)
            {
                _right = _root = root;
                _action = _root == null ? Action.End : Action.Right;
            }

            public bool MoveNext()
            {
                switch (_action)
                {
                    case Action.Right:
                        _current = _right;

                        while (_current.Left != null)
                        {
                            _current = _current.Left;
                        }

                        _right = _current.Right;
                        _action = _right != null ? Action.Right : Action.Parent;

                        return true;

                    case Action.Parent:
                        while (_current.Parent != null)
                        {
                            AvlNode previous = _current;

                            _current = _current.Parent;

                            if (_current.Left == previous)
                            {
                                _right = _current.Right;
                                _action = _right != null ? Action.Right : Action.Parent;

                                return true;
                            }
                        }

                        _action = Action.End;

                        return false;

                    default:
                        return false;
                }
            }

            public void Reset()
            {
                _right = _root;
                _action = _root == null ? Action.End : Action.Right;
            }

            public TValue Current
            {
                get
                {
                    return _current.Value;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public void Dispose()
            {
            }

            enum Action
            {
                Parent,
                Right,
                End
            }
        }
    }
}