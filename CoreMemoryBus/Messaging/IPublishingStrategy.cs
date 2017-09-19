using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public interface IPublishingStrategy
    {
        void Publish(Message message);
    }
}
