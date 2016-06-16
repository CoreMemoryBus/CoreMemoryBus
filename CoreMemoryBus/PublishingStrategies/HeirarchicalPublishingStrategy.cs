using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;

namespace CoreMemoryBus.PublishingStrategies
{
    public class HeirarchicalPublishingStrategy : PublishingStrategy, IPublishingStrategy
    {
        public HeirarchicalPublishingStrategy(MessageHandlerDictionary messageHandlers)
            : base(messageHandlers)
        { }

        public void Publish(Message message)
        {
            var msgType = message.GetType();
            PublishToProxies(message, msgType);
            do
            {
                msgType = msgType.BaseType;
                PublishToProxies(message, msgType);
            }
            while (msgType != typeof(Message));
        }
    }
}