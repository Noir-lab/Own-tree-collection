using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tree_lab_twelve
{
    public class BinaryNode<T> where T : IComparable, ICloneable
    {
        public T data { get; set; }
        public BinaryNode<T>? left { get; set; }
        public BinaryNode<T>? right { get; set; }

        public BinaryNode(T value)
        {
            data = value;
            left = null;
            right = null;
        }

        public override string? ToString() => data?.ToString();
    }
}