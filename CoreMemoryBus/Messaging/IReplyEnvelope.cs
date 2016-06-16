namespace CoreMemoryBus.Messaging
{
    public interface IReplyEnvelope
    {
        void ReplyWith<T>(T message) where T : Messages.Message;
    }
}