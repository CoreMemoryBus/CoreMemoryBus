using CoreMemoryBus.Util;

namespace CoreMemoryBus.Handlers
{
    public class TimedMessageHandler : IHandle<MessageTimer.SetLoggingThreshold>,
                                       IHandle<MessageTimer.RequestLoggingThreshold>
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

        public void Handle(MessageTimer.RequestLoggingThreshold message)
        {
            var response = new MessageTimer.LoggingThresholdResponse(message.CorrelationId, _messageTimer.LogThreshold);
            message.Reply.ReplyWith(response);
        }
    }
}