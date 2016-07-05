namespace CoreMemoryBus.Messages
{
    public interface IAccessControlledMessage
    {
        string[] Principals { get; }
    }
}