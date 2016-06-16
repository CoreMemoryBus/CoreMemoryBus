using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class PublisherReplyEnvelope : IReplyEnvelope
    {
        public IPublisher Publisher { get; private set; }

        public PublisherReplyEnvelope(IPublisher publisher)
        {
            Publisher = publisher;
        }

        public void ReplyWith<T>(T message) where T : Message
        {
            Publisher.Publish(message);
        }
    }
}