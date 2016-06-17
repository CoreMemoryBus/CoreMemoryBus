using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class MessageHandlerProxy<T> : IMessageHandlerProxy where T:Message
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

        public bool IsSame<T1>(object messageHandler)
        {
            return ReferenceEquals(MessageHandler, messageHandler);
        }
    }
}