﻿using CoreMemoryBus.Handlers;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public interface ISubscriber
    {
        void Subscribe<T>(IHandle<T> messageHandler) where T : Message;
        void Subscribe(object messageHandler);
        void Subscribe<T>(IHandle<T> messageHandler, IMessageHandlerProxy proxy) where T : Message;

        void Unsubscribe<T>(IHandle<T> messageHandler) where T : Message;
        void Unsubscribe(object messageHandler);
    }
}
