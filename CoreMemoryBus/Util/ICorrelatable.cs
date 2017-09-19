namespace CoreMemoryBus.Util
{
    public interface ICorrelatable<THashKey>
    {
        THashKey CorrelationId { get; }
    }
}
