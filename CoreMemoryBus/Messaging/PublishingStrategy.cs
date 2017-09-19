using System;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class PublishingStrategy
    {
        protected MessageHandlerDictionary MessageHandlers { get; }

        protected PublishingStrategy(MessageHandlerDictionary messageHandlers)
        {
            MessageHandlers = messageHandlers;
        }

        protected bool TryPublish(Message message, Type msgType)
        {
            MessageHandlerProxies proxies;
            if (MessageHandlers.TryGetValue(msgType, out proxies))
            {
                // permit re-entrancy while publishing
                var nonReentrantProxies = new MessageHandlerProxies(proxies);
                nonReentrantProxies.Publish(message);
                return true;
            }

            return false;
        }
    }
}
