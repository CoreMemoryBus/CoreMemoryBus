using System;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;

namespace CoreMemoryBus
{
    public static class MessageTimer
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

        public class RequestLoggingThreshold : Message, ICorrelatedMessage
        {
            public RequestLoggingThreshold(Guid correlationId, IReplyEnvelope reply)
            {
                CorrelationId = correlationId;
                Reply = reply;
            }

            public Guid CorrelationId { get; private set; }

            public IReplyEnvelope Reply { get; private set; }
        }

        public class LoggingThresholdResponse : Message, ICorrelatedMessage
        {
            public LoggingThresholdResponse(Guid correlationId, TimeSpan loggingThreshold)
            {
                CorrelationId = correlationId;
                LoggingThreshold = loggingThreshold;
            }

            public Guid CorrelationId { get; private set; }

            public TimeSpan LoggingThreshold { get; private set; }
        }
    }
}