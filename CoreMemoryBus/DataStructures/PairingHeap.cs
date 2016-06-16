using System;
using System.Collections.Generic;

namespace CoreMemoryBus.DataStructures
{
    ///https://en.wikipedia.org/wiki/Pairing_heap
    public class PairingHeap<T>
    {
        public class HeapNode
        {
            public T Item;
            public HeapNode SubHeaps;
            public HeapNode Next;
        }

        public interface IHeapNodeFactory
        {
            HeapNode Acquire();
            void Release(HeapNode oldNode);
        }

        private class DefaultFactory : IHeapNodeFactory
        {
            public HeapNode Acquire() { return new HeapNode(); }
            public void Release(HeapNode oldNode) { }
        }

        public HeapNode _root;
        private int _count;
        private readonly Func<T, T, bool> _compare;
        private readonly IHeapNodeFactory _nodeFactory;

        private static readonly IHeapNodeFactory defaultFactory = new DefaultFactory();

        public PairingHeap(IEnumerable<T> items, Func<T, T, bool> compareFn, IHeapNodeFactory nodeFactory)
        {
            _compare = compareFn;
            _nodeFactory = nodeFactory;
            AddItems(items);
        }

        public PairingHeap(IEnumerable<T> items, Func<T, T, bool> compareFn)
            : this(items, compareFn, defaultFactory)
        { }

        public PairingHeap(IEnumerable<T> items, IComparer<T> comparer)
            : this(items, comparer, defaultFactory)
        { }

        public PairingHeap(IEnumerable<T> items, IComparer<T> comparer, IHeapNodeFactory nodeFactory)
            : this(items, ConvertComparer(comparer), nodeFactory)
        { }

        private static Func<T, T, bool> ConvertComparer(IComparer<T> comparer)
        {
            var comp = comparer ?? Comparer<T>.Default;
            return (x, y) => comp.Compare(x, y) < 0;
        }

        private void AddItems(IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    Add(item);
                }
            }
        }

        public int Count { get { return _count; } }

        public void Add(T newItem)
        {
            var newNode = _nodeFactory.Acquire();
            newNode.Item = newItem;
            _root = Merge(_root, newNode);
            _count += 1;
        }

        private HeapNode Merge(HeapNode leftNode, HeapNode rightNode)
        {
            if (leftNode == null)
            {
                return rightNode;
            }

            if (rightNode == null)
            {
                return leftNode;
            }

            if (_compare(leftNode.Item, rightNode.Item))
            {
                rightNode.Next = leftNode.SubHeaps;
                leftNode.SubHeaps = rightNode;
                return leftNode;
            }

            leftNode.Next = rightNode.SubHeaps;
            rightNode.SubHeaps = leftNode;
            return rightNode;
        }

        public T FindMin()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }

            return _root.Item;
        }

        public T DeleteMin()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException();
            }

            var oldRoot = _root;
            var res = _root.Item;
            _root = MergePair(_root.SubHeaps);
            _count -= 1;

            _nodeFactory.Release(oldRoot);
            return res;
        }

        private HeapNode MergePair(HeapNode node)
        {
            HeapNode tail = null;
            HeapNode currentNode = node;

            while (currentNode != null && currentNode.Next != null)
            {
                var n1 = currentNode;
                var n2 = currentNode.Next;
                currentNode = currentNode.Next.Next;

                n1.Next = tail;
                n2.Next = n1;
                tail = n2;
            }

            while (tail != null)
            {
                var n = tail;
                tail = tail.Next.Next;
                currentNode = Merge(currentNode, Merge(n, n.Next));
            }

            return currentNode;
        }
    }
}