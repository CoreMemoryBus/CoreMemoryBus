namespace CoreMemoryBus.Messaging
{
    public interface IMessageHandlerProxy : IPublisher
    {
        bool IsSame(object messageHandler);
    }
}