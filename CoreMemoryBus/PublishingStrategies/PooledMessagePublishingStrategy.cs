using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;

namespace CoreMemoryBus.PublishingStrategies
{
    /// <summary>
    /// This class facilitates message re-use via object pooling to suppress the garbage collector.
    /// When all publishing is complete for an IPooledMessage, it signals to be released back into the object pool.
    /// Non-implementing messages are directly published.
    /// </summary>
    public class PooledMessagePublishingStrategy : IPublishingStrategy
    {
        private readonly IPublishingStrategy _implementation;

        public PooledMessagePublishingStrategy(IPublishingStrategy implementation)
        {
            _implementation = implementation;
        }

        public void Publish(Message message)
        {
            _implementation.Publish(message);

            var pooledMessage = message as IPooledMessage;
            if (pooledMessage != null)
            {
                pooledMessage.Release();
            }
        }
    }
}
