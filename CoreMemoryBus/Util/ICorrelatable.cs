using System;

namespace CoreMemoryBus.Util
{
    public interface ICorrelatable
    {
        Guid CorrelationId { get; }
    }
}