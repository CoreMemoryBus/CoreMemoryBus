using System;

namespace CoreMemoryBus.Messages
{
    public interface IAccessControlMessage
    {
        Type Type { get; }
        string[] Principals { get; }
    }
}
