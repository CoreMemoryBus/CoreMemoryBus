namespace CoreMemoryBus.Messaging
{
    public class MessageHandlerProxy<T> : IMessageHandlerProxy where T:Messages.Message
    {
        private IHandle<T> MessageHandler { get; set; }

        public MessageHandlerProxy(IHandle<T> messageHandler)
        {
            MessageHandler = messageHandler;
        }

        public void Publish(Messages.Message message)
        {
            MessageHandler.Handle((T)message);
        }

        public bool IsSame<T1>(object messageHandler)
        {
            return ReferenceEquals(MessageHandler, messageHandler);
        }
    }
}