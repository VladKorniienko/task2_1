using System;
using System.Collections.Generic;

namespace Korniienko_Task3
{
    public enum Side
    {
        Left,
        Right
    }

    public class BinaryTreeNode<T>  where T : IComparable
    {
        
        public BinaryTreeNode(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public BinaryTreeNode<T> LeftNode { get; set; }
        public BinaryTreeNode<T> RightNode { get; set; }
        public BinaryTreeNode<T> ParentNode { get; set; }

        /// <summary>
        /// Расположение узла относительно его родителя
        /// </summary>
        
        public Side? NodeSide
        {
            get
            {
                if (ParentNode == null)
                    return (Side?) null;
                else
                    return ParentNode.LeftNode == this
                        ? Side.Left
                        : Side.Right;
            }
        }


        public override string ToString() => Data.ToString();

    }
}
