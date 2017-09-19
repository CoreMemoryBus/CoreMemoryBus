using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class NullMessageSink : IMessageSink
    {
        public void ReceiveMessage(Message msg)
        {}
    }
}
