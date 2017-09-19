using System;

namespace CoreMemoryBus.Messages
{
    public interface IAccessControlMessage
    {
        string Principal { get; }
        Type Type { get; }
    }
}
