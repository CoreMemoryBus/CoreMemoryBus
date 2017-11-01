using System;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Messages
{
    public interface IAccessControlMessage : IAclAdminMessage
    {
        Type ControlledMessageType { get; }
        Principal Principal { get; }
    }
}
