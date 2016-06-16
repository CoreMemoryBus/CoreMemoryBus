using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Sagas
{
    public interface ISaga : IPublisher, ICorrelatable
    {
        bool IsComplete { get; }
    }
}