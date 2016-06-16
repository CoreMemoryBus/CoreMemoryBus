using System;
using System.Collections.Concurrent;

namespace CoreMemoryBus.DataStructures
{
    /// <summary>
    /// An object pool is created to manage recycling of object references, thereby suppressing garbage collection.
    /// This can be used to increase application performance under specific circumstances.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T>
    {
        protected int MaxCapacity { get; private set; }
        protected Func<ObjectPool<T>, T> ObjectFactory { get; private set; }

        private readonly ConcurrentBag<T> _container = new ConcurrentBag<T>();

        public ObjectPool(int maxCapacity, Func<ObjectPool<T>, T> objectFactory)
        {
            MaxCapacity = maxCapacity;
            ObjectFactory = objectFactory;

            for (int i = 0; i < maxCapacity; ++i)
            {
                _container.Add(objectFactory(this));
            }
        }

        public T Borrow()
        {
            T result;
            if (_container.TryTake(out result))
            {
                return result;
            }

            throw new MaxCapacityException(MaxCapacity);
        }

        public void Return(T item)
        {
            _container.Add(item);
        }

        public int Count { get { return _container.Count; } }
    }
}