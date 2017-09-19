using System;

namespace CoreMemoryBus.Messages
{
    public interface IUniqueMessage
    {
        Guid Id { get; }
    }

    public interface IUniqueMessage<T>
    {
        T Id { get; }
    }
}
