namespace CoreMemoryBus
{
    public interface IHandle<T>
    {
        void Handle(T message);
    }
}