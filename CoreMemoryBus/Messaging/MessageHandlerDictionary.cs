using System;
using System.Collections.Generic;

namespace CoreMemoryBus.Messaging
{
    public class MessageHandlerDictionary : Dictionary<Type, MessageHandlerProxies>
    { }
}