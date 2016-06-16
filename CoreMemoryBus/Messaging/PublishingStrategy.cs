﻿using System;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class PublishingStrategy
    {
        protected MessageHandlerDictionary MessageHandlers { get; private set; }

        protected PublishingStrategy(MessageHandlerDictionary messageHandlers)
        {
            MessageHandlers = messageHandlers;
        }

        protected void PublishToProxies(Message message, Type msgType)
        {
            MessageHandlerProxies proxies;
            if (MessageHandlers.TryGetValue(msgType, out proxies))
            {
                proxies.Publish(message);
            }
        }
    }
}