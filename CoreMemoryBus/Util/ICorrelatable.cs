using System;

namespace CoreMemoryBus.Util
{
    public interface ICorrelatable<THashKey>
    {
        THashKey CorrelationId { get; }
    }

    public interface ICorrelatable : ICorrelatable<Guid>
    {}
}