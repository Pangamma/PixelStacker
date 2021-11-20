// Accord Machine Learning Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © César Souza, 2009-2017
// cesarsouza at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//

namespace PixelStacker.Logic.Collections
{
    using System;
    using System.Text;


    /// <summary>
    ///   K-dimensional tree node (for <see cref="KDTree{T}"/>).
    /// </summary>
    /// 
    /// <typeparam name="T">The class type node content.</typeparam>
    /// 
    /// <seealso cref="ColorTreeNodePS"/>
    /// <seealso cref="ColorTreeNode{T}"/>
    /// <seealso cref="BinaryNode{TNode}"/>
    /// 
    [Serializable]
    public class ColorTreeNode<T> : IComparable<ColorTreeNode<T>>, IEquatable<ColorTreeNode<T>> // TODO: Try to remove IEquatable
    {
        /// <summary>
        ///   Gets or sets the value being stored at this node.
        /// </summary>
        /// 
        public T Value { get; set; }

        /// <summary>
        ///   Gets or sets the position of 
        ///   the node in spatial coordinates.
        /// </summary>
        /// 
        public double[] Position { get; set; }

        /// <summary>
        ///   Gets or sets the dimension index of the split. This value is a
        ///   index of the <see cref="Position"/> vector and as such should
        ///   be higher than zero and less than the number of elements in <see cref="Position"/>.
        /// </summary>
        /// 
        public int Axis { get; set; }

        public override string ToString()
        {
            if (Position == null)
                return "(null)";

            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < Position.Length; i++)
            {
                sb.Append(Position[i]);
                if (i < Position.Length - 1)
                    sb.Append(",");
            }
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.
        /// </returns>
        public int CompareTo(ColorTreeNode<T> other)
        {
            return this.Position[this.Axis].CompareTo(other.Position[other.Axis]);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(ColorTreeNode<T> other) // TODO: Try to remove IEquatable
        {
            return this == other;
        }

        /// <summary>
        ///   Gets or sets the left subtree of this node.
        /// </summary>
        /// 
        public ColorTreeNode<T> Left { get; set; }

        /// <summary>
        ///   Gets or sets the right subtree of this node.
        /// </summary>
        /// 
        public ColorTreeNode<T> Right { get; set; }

        /// <summary>
        ///   Gets whether this node is a leaf (has no children).
        /// </summary>
        /// 
        public bool IsLeaf
        {
            get { return Left == default(ColorTreeNode<T>) && Right == default(ColorTreeNode<T>); }
        }


        /// <summary>
        ///   Gets or sets the collection of child nodes
        ///   under this node.
        /// </summary>
        /// 
        public ColorTreeNode<T>[] Children
        {
            get { return new[] { Left, Right }; }
            set
            {
                if (value.Length != 2)
                    throw new ArgumentException("The array must have length 2.", "value");
                Left = value[0];
                Right = value[1];
            }
        }
    }
}
