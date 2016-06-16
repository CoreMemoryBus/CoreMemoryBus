using System;

namespace CoreMemoryBus.PublishingStrategies
{
    public interface IAccessControlMessage
    {
        string Principal { get; }
        Type Type { get; }
    }
}