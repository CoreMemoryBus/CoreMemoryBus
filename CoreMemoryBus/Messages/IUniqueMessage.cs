using System;

namespace CoreMemoryBus.Messages
{
    public interface IUniqueMessage
    {
        Guid Id { get; }
    }
}