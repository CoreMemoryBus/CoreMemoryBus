using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public interface IReplyEnvelope
    {
        void ReplyWith<T>(T message) where T : Message;
    }
}
