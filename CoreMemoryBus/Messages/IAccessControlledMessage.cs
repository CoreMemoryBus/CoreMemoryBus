using CoreMemoryBus.Util;

namespace CoreMemoryBus.Messages
{
    public interface IAccessControlledMessage
    {
        User User { get; }
    }
}
