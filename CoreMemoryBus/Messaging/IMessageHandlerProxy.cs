namespace CoreMemoryBus.Messaging
{
    public interface IMessageHandlerProxy : IPublisher
    {
        bool IsSame<T>(object messageHandler);
    }
}