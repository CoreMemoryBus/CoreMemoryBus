using CoreMemoryBus.Handlers;
using CoreMemoryBus.Messages;
using System;

namespace CoreMemoryBus.Messaging
{
    public class MessageHandlerProxy<T> : IMessageHandlerProxy<T> where T : Message
    {
        private IHandle<T> MessageHandler { get; set; }

        public MessageHandlerProxy(IHandle<T> messageHandler)
        {
            MessageHandler = messageHandler;
        }

        public void Publish(Message message)
        {
            MessageHandler.Handle((T)message);
        }

        public bool IsSame(object messageHandler)
        {
            return ReferenceEquals(MessageHandler, messageHandler);
        }
    }
}
