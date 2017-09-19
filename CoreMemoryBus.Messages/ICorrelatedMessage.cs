namespace CoreMemoryBus.Messages
{
    public interface ICorrelatedMessage<THashKey>
    {
        THashKey CorrelationId { get; }
    }
}
