namespace CoreMemoryBus.Handlers
{
    /// <summary>
    /// Implementors of this interface cause a Saga to be instantiated and initialised.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAmTriggeredBy<in T>
    {
        void Handle(T message);
    }
}
