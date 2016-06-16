using System.Collections.Generic;

namespace CoreMemoryBus.DataStructures
{
    public interface IHashSetDictionary<TKey, TElement> : IEnumerable<ISetGrouping<TKey, TElement>>
    {
        bool Contains(TKey key);
        int Count { get; }
        ISetGrouping<TKey, TElement> this[TKey key] { get; }
    }
}