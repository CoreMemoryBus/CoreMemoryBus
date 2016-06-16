using CoreMemoryBus.PublishingStrategies;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Handlers
{
    public class TimedMessageHandler : IHandle<MessageTimer.SetLoggingThreshold>
    {
        private readonly IMessageTimer _messageTimer;

        public TimedMessageHandler(IMessageTimer messageTimer)
        {
            Ensure.ArgumentIsNotNull(messageTimer,"messageTimer");
            _messageTimer = messageTimer;
        }

        public void Handle(MessageTimer.SetLoggingThreshold message)
        {
            _messageTimer.LogThreshold = message.LoggingThreshold;
        }
    }
}