using System;

namespace CoreMemoryBus
{
    public interface ICorrelatedMessage
    {
        Guid CorrelationId { get; }
    }
}