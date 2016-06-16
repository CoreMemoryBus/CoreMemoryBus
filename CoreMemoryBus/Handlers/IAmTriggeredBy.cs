namespace CoreMemoryBus
{
    public interface IAmTriggeredBy<T>
    {
        void Handle(T message);
    }
}