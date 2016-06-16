namespace CoreMemoryBus.PublishingStrategies
{
    public interface IAccessControlledMessage
    {
        string[] Principals { get; }
    }
}