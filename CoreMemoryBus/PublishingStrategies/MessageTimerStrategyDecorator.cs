using System.Diagnostics;
using CoreMemoryBus.Logger;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.PublishingStrategies
{
    public class MessageTimerStrategyDecorator : IPublishingStrategy
    {
        private readonly IMessageTimer _messageTimer;
        private readonly IPublishingStrategy _implementation;
        private readonly ILogger _logger;
        
        public MessageTimerStrategyDecorator(IMessageTimer messageTimer, ILogger logger, IPublishingStrategy implementation)
        {
            _messageTimer = messageTimer;
            _implementation = implementation;
            _logger = logger;
        }

        public void Publish(Message message)
        {
            var stopwatch = _messageTimer.CreateStopwatch();
            stopwatch.Start();
            try
            {
                _implementation.Publish(message);

                var elapsed = stopwatch.Elapsed;
                if (elapsed > _messageTimer.LogThreshold)
                {
                    _logger.Warn("Slow publication for message type:{0}. > {1}(s)", message.GetType(),
                        _messageTimer.LogThreshold);
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }
    }
}