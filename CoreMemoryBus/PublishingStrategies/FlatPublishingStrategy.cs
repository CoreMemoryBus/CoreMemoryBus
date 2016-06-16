using CoreMemoryBus.Messaging;

namespace CoreMemoryBus.PublishingStrategies
{
    public class FlatPublishingStrategy : PublishingStrategy, IPublishingStrategy
    {
        public FlatPublishingStrategy(MessageHandlerDictionary messageHandlers)
            : base(messageHandlers)
        { }

        public void Publish(Messages.Message message)
        {
            PublishToProxies(message, message.GetType());
        }
    }
}