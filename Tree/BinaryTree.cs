using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace tree_lab_twelve
{
    public class BinaryTree<T> :
        IEnumerable<T>, ICollection<T>, ICloneable
        where T : IComparable, ICloneable
    {
        private BinaryNode<T>? root { get; set; }
        private List<T>? elements;
        public int Count { get; private set; }
        public bool IsReadOnly => false;
        public IComparer<T> Comparer { get; private set; }

        public BinaryTree()
        {
            root = null;
            elements = new List<T>();
        }

        public BinaryTree(BinaryTree<T> tree) : this(tree.Comparer)
        {
            if (tree.Count != 0)
            {
                this.root = CloneNode(tree.root);
                this.Count = tree.Count;
            }
        }

        public BinaryTree(IComparer<T>? comparer) => this.Comparer = comparer ?? Comparer<T>.Default;

        public BinaryTree(IEnumerable<T> collection, IComparer<T>? comparer = null) : this(comparer) => Add(collection);

        public T? Find(T item)
        {
            var itemNode = FindNode(item, root);
            if (itemNode != null)
                return itemNode.data;
            return default;
        }

        private static T GetMinValue(BinaryNode<T> currentNode)
        {
            while (currentNode.left != null)
                currentNode = currentNode.left;

            return currentNode.data;
        }

        public BinaryNode<T>? FindNode(T data, BinaryNode<T>? startWithNode = null)
        {
            if (startWithNode == null)
                return null;

            int result = Comparer.Compare(data, startWithNode.data);

            if (result == 0)
                return startWithNode;
            else if (result < 0)
                return FindNode(data, startWithNode.left);
            else
                return FindNode(data, startWithNode.right);

        }

        public virtual void Add(T data)
        {
            root = Add(root, data);
            Count++;
        }

        public BinaryNode<T> Add(BinaryNode<T>? currentNode, T data)
        {
            if (currentNode == null)
                return new BinaryNode<T>(data);

            int result = Comparer.Compare(data, currentNode.data);

            if (result == 0)
                Count--;

            else if (result > 0)
                currentNode.right = Add(currentNode.right, data);
            else if (result < 0)
                currentNode.left = Add(currentNode.left, data);
            return currentNode;
        }

        public virtual void Add(IEnumerable<T> collection)
        {
            foreach (var value in collection)
                Add(value);
        }

        public virtual void Clear()
        {
            root = null;
            Count = 0;
        }

        public object Clone() => (object)new BinaryTree<T>(this);

        public bool Contains(T item) => FindNode(item, root) is not null;

        public void CopyTo(T[]? array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException("Insufficient space in target array.");
            }

            foreach (var value in this)
                array[arrayIndex++] = value;
        }

        public IEnumerator<T> GetEnumerator() => InOrder(root).GetEnumerator();

        public virtual bool Remove(T data)
        {
            var initialCount = Count;
            root = Remove(root, data);
            return Count < initialCount;
        }
        private BinaryNode<T>? Remove(BinaryNode<T>? currentNode, T? data)
        {
            if (currentNode == null)
                return null;

            int result = Comparer.Compare(data, currentNode.data);

            if (result < 0)
                currentNode.left = Remove(currentNode.left, data);
            else if (result > 0)
                currentNode.right = Remove(currentNode.right, data);
            else
            {
                if (currentNode.left == null)
                {
                    --Count;
                    return currentNode.right;
                }
                else if (currentNode.right == null)
                {
                    --Count;
                    return currentNode.left;
                }

                currentNode.data = GetMinValue(currentNode.right);
                currentNode.right = Remove(currentNode.right, currentNode.data);
            }
            return currentNode;
        }

        private string? GetDebuggerDisplay() => ToString();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<T> InOrder(BinaryNode<T>? node)
        {
            if (node != null)
            {
                foreach (var item in InOrder(node.left))
                {
                    yield return item;
                }

                yield return node.data;

                foreach (var item in InOrder(node.right))
                {
                    yield return item;
                }
            }
        }

        public object ShallowCopy() => (BinaryTree<T>)this.MemberwiseClone();

        private BinaryNode<T>? CloneNode(BinaryNode<T>? node)
        {
            if (node == null)
                return null;

            var clonedNode = new BinaryNode<T>((T)node.data.Clone())
            {
                left = CloneNode(node.left),
                right = CloneNode(node.right)
            };
            return clonedNode;
        }

        public void PrintTree()
        {
            if (Count == 0)
                Console.WriteLine("Tree is empty");
            else
                Console.WriteLine(TraversePreOrder(root));

        }

        public String TraversePreOrder(BinaryNode<T>? root)
        {

            if (root == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(root.data);

            String pointerRight = "└──";
            String pointerLeft = (root.right != null) ? "├──" : "└──";

            TraverseNodes(sb, "", pointerLeft, root.left, root.right != null);
            TraverseNodes(sb, "", pointerRight, root.right, false);

            return sb.ToString();
        }

        public void TraverseNodes(StringBuilder sb, String padding, String pointer, BinaryNode<T>? node,
  Boolean hasRightSibling)
        {
            if (node != null)
            {
                sb.Append("\n");
                sb.Append(padding);
                sb.Append(pointer);
                sb.Append(node.data);

                StringBuilder paddingBuilder = new StringBuilder(padding);
                if (hasRightSibling)
                {
                    paddingBuilder.Append("│  ");
                }
                else
                {
                    paddingBuilder.Append("  ");
                }

                String paddingForBoth = paddingBuilder.ToString();
                String pointerRight = "└──";
                String pointerLeft = (node.right != null) ? "├──" : "└──";

                TraverseNodes(sb, paddingForBoth, pointerLeft, node.left, node.right != null);
                TraverseNodes(sb, paddingForBoth, pointerRight, node.right, false);
            }
        }

    }
}