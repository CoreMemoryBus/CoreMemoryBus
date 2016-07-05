using System;

namespace CoreMemoryBus.Messages
{
    public interface ICorrelatedMessage
    {
        Guid CorrelationId { get; }
    }
}