using System;
using System.Collections;
using System.Collections.Generic;

namespace Korniienko_Task3
{
    public enum TraversalMode
    {
        PreOrder,
        InOrder,
        PostOrder

    }

    public class BinaryTree<T> where T : IComparable
    {

        private TraversalMode _traversalOrder = TraversalMode.PreOrder;

        public BinaryTreeNode<T> RootNode { get; set; }
        public delegate void TreeHandler(string message);
        public event TreeHandler Notify;

        public TraversalMode TraversalOrder
        {
            get => _traversalOrder;
            set => _traversalOrder = value;
        }

        public BinaryTreeNode<T> FindNode(T data, BinaryTreeNode<T> startWithNode = null)
        {
            startWithNode = startWithNode ?? RootNode;
            int result;
            if ((result = data.CompareTo(startWithNode.Data)) == 0)
                return startWithNode;
            else if (result < 0)
                return startWithNode.LeftNode == null
                    ? null
                    : FindNode(data, startWithNode.LeftNode);
            else
                return startWithNode.RightNode == null
                    ? null
                    : FindNode(data, startWithNode.RightNode);
        }

        public BinaryTreeNode<T> Add(T data)
        {
            Notify?.Invoke($"Added node: {data.ToString()}");
            return Add(new BinaryTreeNode<T>(data));

        }
        public BinaryTreeNode<T> Add(BinaryTreeNode<T> node, BinaryTreeNode<T> currentNode = null)
        {
            if (RootNode == null)
            {
                node.ParentNode = null;
                return RootNode = node;
            }


            currentNode = currentNode ?? RootNode;
            node.ParentNode = currentNode;

            int result;
            if ((result = node.Data.CompareTo(currentNode.Data)) == 0)
                return currentNode;
            else if (result < 0)
                return currentNode.LeftNode == null
                    ? (currentNode.LeftNode = node)
                    : Add(node, currentNode.LeftNode);
            else
                return currentNode.RightNode == null
                    ? (currentNode.RightNode = node)
                    : Add(node, currentNode.RightNode);
        }


        public void Remove(BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            var currentNodeSide = node.NodeSide;
            //если у узла нет подузлов, можно его удалить
            if (node.LeftNode == null && node.RightNode == null)
            {
                if (currentNodeSide == Side.Left)
                {
                    node.ParentNode.LeftNode = null;
                }
                else
                {
                    node.ParentNode.RightNode = null;
                }
            }
            //если нет левого, то правый ставим на место удаляемого 
            else if (node.LeftNode == null)
            {
                if (currentNodeSide == Side.Left)
                {
                    node.ParentNode.LeftNode = node.RightNode;
                }
                else
                {
                    node.ParentNode.RightNode = node.RightNode;
                }

                node.RightNode.ParentNode = node.ParentNode;
            }
            //если нет правого, то левый ставим на место удаляемого 
            else if (node.RightNode == null)
            {
                if (currentNodeSide == Side.Left)
                {
                    node.ParentNode.LeftNode = node.LeftNode;
                }
                else
                {
                    node.ParentNode.RightNode = node.LeftNode;
                }

                node.LeftNode.ParentNode = node.ParentNode;
            }
            //если оба дочерних присутствуют, 
            //то правый становится на место удаляемого,
            //а левый вставляется в правый
            else
            {
                switch (currentNodeSide)
                {
                    case Side.Left:
                        node.ParentNode.LeftNode = node.RightNode;
                        node.RightNode.ParentNode = node.ParentNode;
                        Add(node.LeftNode, node.RightNode);
                        break;
                    case Side.Right:
                        node.ParentNode.RightNode = node.RightNode;
                        node.RightNode.ParentNode = node.ParentNode;
                        Add(node.LeftNode, node.RightNode);
                        break;
                    default:
                        var bufLeft = node.LeftNode;
                        var bufRightLeft = node.RightNode.LeftNode;
                        var bufRightRight = node.RightNode.RightNode;
                        node.Data = node.RightNode.Data;
                        node.RightNode = bufRightRight;
                        node.LeftNode = bufRightLeft;
                        Add(bufLeft, node);
                        break;
                }
            }
        }


        public void Remove(T data)
        {
            Notify?.Invoke($"Removed node: {data.ToString()}");
            var foundNode = FindNode(data);
            Remove(foundNode);
        }

        public void PrintTree()
        {
            PrintTree(RootNode);
        }

        /// <summary>
        /// Вывод бинарного дерева начиная с указанного узла
        /// </summary>
        private void PrintTree(BinaryTreeNode<T> startNode, string indent = "", Side? side = null)
        {
            if (startNode == null) return;
            var nodeSide = side == null ? "ROOT" : side == Side.Left ? "L" : "R";
            Console.WriteLine($"{indent} [{nodeSide}]- {startNode.Data.ToString()}");
            indent += new string(' ', 4);
            //рекурсивный вызов для левой и правой веток
            PrintTree(startNode.LeftNode, indent, Side.Left);
            PrintTree(startNode.RightNode, indent, Side.Right);
        }

        public IEnumerator<T> GetEnumerator()
        {
            switch (TraversalOrder)
            {
                case TraversalMode.PreOrder:
                    return GetPreOrderEnumerator();
                case TraversalMode.PostOrder:
                    return GetPostOrderEnumerator();
                case TraversalMode.InOrder:
                    return GetInOrderEnumerator();
                default:
                    return GetPreOrderEnumerator();
            }
        }
        public IEnumerator<T> GetInOrderEnumerator()
        {
            return new BinaryTreeInOrderEnumerator(this);
        }
        public IEnumerator<T> GetPostOrderEnumerator()
        {
            return new BinaryTreePostOrderEnumerator(this);
        }
        public IEnumerator<T> GetPreOrderEnumerator()
        {
            return new BinaryTreePreOrderEnumerator(this);
        }

        internal class BinaryTreePreOrderEnumerator : IEnumerator<T>
        {
            private BinaryTreeNode<T> current;
            private BinaryTree<T> tree;
            internal Queue<BinaryTreeNode<T>> traverseQueue;

            public BinaryTreePreOrderEnumerator(BinaryTree<T> tree)
            {
                this.tree = tree;

                //Build queue
                traverseQueue = new Queue<BinaryTreeNode<T>>();
                visitNode(this.tree.RootNode);
            }

            private void visitNode(BinaryTreeNode<T> node)
            {
                if (node == null)
                    return;
                else
                {
                    traverseQueue.Enqueue(node);
                    visitNode(node.LeftNode);
                    visitNode(node.RightNode);
                }
            }

            public T Current => current.Data;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                current = traverseQueue.Count > 0 ? traverseQueue.Dequeue() : null;

                return (current != null);
            }
        }

        internal class BinaryTreeInOrderEnumerator : IEnumerator<T>
        {
            private BinaryTreeNode<T> current;
            private BinaryTree<T> tree;
            internal Queue<BinaryTreeNode<T>> traverseQueue;

            public BinaryTreeInOrderEnumerator(BinaryTree<T> tree)
            {
                this.tree = tree;

                //Build queue
                traverseQueue = new Queue<BinaryTreeNode<T>>();
                visitNode(this.tree.RootNode);
            }

            private void visitNode(BinaryTreeNode<T> node)
            {
                if (node == null)
                    return;
                else
                {
                    visitNode(node.LeftNode);
                    traverseQueue.Enqueue(node);
                    visitNode(node.RightNode);
                }
            }

            public T Current
            {
                get { return current.Data; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                if (traverseQueue.Count > 0)
                    current = traverseQueue.Dequeue();
                else
                    current = null;

                return (current != null);
            }
        }
        internal class BinaryTreePostOrderEnumerator : IEnumerator<T>
        {
            private BinaryTreeNode<T> current;
            private BinaryTree<T> tree;
            internal Queue<BinaryTreeNode<T>> traverseQueue;

            public BinaryTreePostOrderEnumerator(BinaryTree<T> tree)
            {
                this.tree = tree;

                //Build queue
                traverseQueue = new Queue<BinaryTreeNode<T>>();
                visitNode(this.tree.RootNode);
            }

            private void visitNode(BinaryTreeNode<T> node)
            {
                if (node == null)
                    return;
                else
                {
                    visitNode(node.LeftNode);
                    visitNode(node.RightNode);
                    traverseQueue.Enqueue(node);
                }
            }

            public T Current
            {
                get { return current.Data; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                current = null;
                tree = null;
            }

            public void Reset()
            {
                current = null;
            }

            public bool MoveNext()
            {
                if (traverseQueue.Count > 0)
                    current = traverseQueue.Dequeue();
                else
                    current = null;

                return (current != null);
            }
        }


    }
}


