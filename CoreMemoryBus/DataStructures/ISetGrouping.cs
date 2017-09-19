using System.Collections.Generic;

namespace CoreMemoryBus.DataStructures
{
    public interface ISetGrouping<out TKey, TElement> : ISet<TElement>
    {
        TKey Key { get; }
    }
}
