using System;

namespace CoreMemoryBus.Messages
{
    public interface ICorrelatedMessage<THashKey>
    {
        THashKey CorrelationId { get; }
    }

    public interface ICorrelatedMessage : ICorrelatedMessage<Guid>
    {}
}