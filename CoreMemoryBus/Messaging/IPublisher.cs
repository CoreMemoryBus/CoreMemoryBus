namespace CoreMemoryBus.Messaging
{
    public interface IPublisher
    {
        void Publish(Messages.Message message);
    }
}