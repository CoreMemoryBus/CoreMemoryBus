using System.Collections.Generic;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;

namespace CoreMemoryBus.PublishingStrategies
{
    /// <summary>
    /// This class facilitates the unique execution of a single message. It 
    /// records the unique identifier of every IUniqueMessage and allows 
    /// publication only once. Non-implementing messages are directly published.
    /// </summary>
    public class UniqueMessagePublishingStrategy<TKey> : IPublishingStrategy
    {
        private readonly HashSet<TKey> _uniqueMessages;
        private readonly IPublishingStrategy _implementation;

        public UniqueMessagePublishingStrategy(IPublishingStrategy implementation)
        {
            _implementation = implementation;
            _uniqueMessages = new HashSet<TKey>();
        }

        public void Publish(Message message)
        {
            var unique = message as IUniqueMessage<TKey>;
            if (unique != null)
            {
                if (!_uniqueMessages.Contains(unique.Id))
                {
                    _implementation.Publish(message);
                    _uniqueMessages.Add(unique.Id);
                }

                return;
            }

            _implementation.Publish(message);
        }

        public int Count { get { return _uniqueMessages.Count; } }
    }
}
