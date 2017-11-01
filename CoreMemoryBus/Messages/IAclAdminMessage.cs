using CoreMemoryBus.Util;

namespace CoreMemoryBus.Messages
{
    public interface IAclAdminMessage
    {
        User AdminUser { get; }
    }
}
