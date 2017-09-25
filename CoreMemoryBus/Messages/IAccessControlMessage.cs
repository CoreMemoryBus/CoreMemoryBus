using System;

namespace CoreMemoryBus.Messages
{
    public interface IAccessControlMessage : IAclAdminMessage
    {
        Type ControlledMessageType { get; }
        string[] Principals { get; }
    }
}
