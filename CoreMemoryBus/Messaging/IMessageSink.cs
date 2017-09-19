using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    /// <summary>
    /// A message sink is a potential endpoint in the handling of a message.
    /// </summary>
    public interface IMessageSink
    {
        void ReceiveMessage(Message msg);
    }
}
