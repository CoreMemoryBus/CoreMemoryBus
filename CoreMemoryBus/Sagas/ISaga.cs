using System;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Sagas
{
    public interface ISaga : IPublisher, ICorrelatable<Guid>
    {
        bool IsComplete { get; }
    }
}