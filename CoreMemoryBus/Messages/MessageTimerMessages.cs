using CoreMemoryBus.Messaging;
using System;

namespace CoreMemoryBus.Messages
{
    public static class MessageTimerMessages
    {
        public class SetLoggingThreshold : Message, IUniqueMessage
        {
            public SetLoggingThreshold(Guid id, TimeSpan loggingThreshold)
            {
                Id = id;
                LoggingThreshold = loggingThreshold;
            }

            public Guid Id { get; private set; }
            public TimeSpan LoggingThreshold { get; private set; }
        }

        public class RequestLoggingThreshold : Message, ICorrelatedMessage<Guid>
        {
            public RequestLoggingThreshold(Guid correlationId, IReplyEnvelope reply)
            {
                CorrelationId = correlationId;
                Reply = reply;
            }

            public Guid CorrelationId { get; private set; }

            public IReplyEnvelope Reply { get; private set; }
        }

        public class LoggingThresholdResponse : Message, ICorrelatedMessage<Guid>
        {
            public LoggingThresholdResponse(Guid correlationId, TimeSpan loggingThreshold)
            {
                CorrelationId = correlationId;
                LoggingThreshold = loggingThreshold;
            }

            public Guid CorrelationId { get; private set; }

            public TimeSpan LoggingThreshold { get; private set; }
        }

        public class SlowPublication : Message
        {
            private Message message;
            private TimeSpan elapsed;

            public SlowPublication(Message message, TimeSpan elapsed)
            {
                this.message = message;
                this.elapsed = elapsed;
            }
        }
    }

}