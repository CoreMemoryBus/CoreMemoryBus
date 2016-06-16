namespace CoreMemoryBus.Messaging
{
    public interface IPublishingStrategy
    {
        void Publish(Messages.Message message);
    }
}