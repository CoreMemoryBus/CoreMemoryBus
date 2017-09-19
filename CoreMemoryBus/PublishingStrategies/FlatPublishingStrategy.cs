using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;

namespace CoreMemoryBus.PublishingStrategies
{
    public class FlatPublishingStrategy : PublishingStrategy, IPublishingStrategy
    {
        public FlatPublishingStrategy(MessageHandlerDictionary messageHandlers, IMessageSink unhandledMessageSink = null)
            : base(messageHandlers)
        {
            UnhandledMessageSink = unhandledMessageSink;
        }

        protected IMessageSink UnhandledMessageSink { get; }

        public void Publish(Message message)
        {
            if (!TryPublish(message, message.GetType()))
            {
                UnhandledMessageSink.ReceiveMessage(message);
            }
        }
    }
}
