﻿using System.Collections.Generic;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class MessageHandlerProxies : List<IMessageHandlerProxy>
    {
        public void Publish(Message message)
        {
            foreach (var proxy in this)
            {
                proxy.Publish(message);
            }
        }
    }
}