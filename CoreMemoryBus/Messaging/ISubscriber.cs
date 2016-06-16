namespace CoreMemoryBus.Messaging
{
    public interface ISubscriber
    {
        void Subscribe<T>(IHandle<T> messageHandler) where T : Messages.Message;
        void Subscribe(object messageHandler);
        void Unsubscribe<T>(IHandle<T> messageHandler) where T : Messages.Message;
    }
}