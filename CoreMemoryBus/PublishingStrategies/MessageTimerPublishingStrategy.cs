using System;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.PublishingStrategies
{
    /// <summary>
    /// This class provides a timing service that records any processed messages that take longer than a particular duration 
    /// to process. Slow messages are wrapped with a SlowPublication message and passed to the slow message sink after handling, 
    /// which could be a logger, histogram or statistics service.
    /// </summary>
    public class MessageTimerPublishingStrategy : IPublishingStrategy
    {
        private readonly IPublishingStrategy _implementation;
        private readonly IMessageTimer _messageTimer;
        private readonly IMessageSink _slowMessageSink;

        public MessageTimerPublishingStrategy(IPublishingStrategy implementation, IMessageTimer messageTimer, IMessageSink slowMessageSink)
        {
            _implementation = implementation;
            _messageTimer = messageTimer;
            _slowMessageSink = slowMessageSink;
        }

        public void Publish(Message message)
        {
            _messageTimer.Restart();
            try
            {
                _implementation.Publish(message);

                var elapsed = _messageTimer.Elapsed;
                if (elapsed > _messageTimer.LogThreshold)
                {
                    _slowMessageSink.ReceiveMessage(new MessageTimerMessages.SlowPublication(message, elapsed));
                }
            }
            finally
            {
                _messageTimer.Stop();
            }
        }
    }
}
