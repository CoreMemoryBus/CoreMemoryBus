using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public interface IMessageHandlerProxy : IPublisher
    {
        bool IsSame(object messageHandler);
    }

    public interface IMessageHandlerProxy<in T> : IMessageHandlerProxy where T:Message
    { }
}
