using CoreMemoryBus.Messages;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Handlers
{
    public class TimedMessageHandler : IHandle<MessageTimerMessages.SetLoggingThreshold>,
                                       IHandle<MessageTimerMessages.RequestLoggingThreshold>
    {
        private readonly IMessageTimer _messageTimer;

        public TimedMessageHandler(IMessageTimer messageTimer)
        {
            Ensure.ArgumentIsNotNull(messageTimer,"messageTimer");
            _messageTimer = messageTimer;
        }

        public void Handle(MessageTimerMessages.SetLoggingThreshold message)
        {
            _messageTimer.LogThreshold = message.LoggingThreshold;
        }

        public void Handle(MessageTimerMessages.RequestLoggingThreshold message)
        {
            var response = new MessageTimerMessages.LoggingThresholdResponse(message.CorrelationId, _messageTimer.LogThreshold);
            message.Reply.ReplyWith(response);
        }
    }
}