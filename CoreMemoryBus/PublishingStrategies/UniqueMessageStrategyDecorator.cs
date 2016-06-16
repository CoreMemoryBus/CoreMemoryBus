using System;
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
    public class UniqueMessageStrategyDecorator : IPublishingStrategy
    {
        private readonly HashSet<Guid> _uniqueMessages;
        private readonly IPublishingStrategy _implementation;

        public UniqueMessageStrategyDecorator(IPublishingStrategy implementation)
        {
            _implementation = implementation;
            _uniqueMessages = new HashSet<Guid>();
        }

        public void Publish(Message message)
        {
            var unique = message as IUniqueMessage;
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