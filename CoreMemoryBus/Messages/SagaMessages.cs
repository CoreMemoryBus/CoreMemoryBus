using System;
using CoreMemoryBus.Messaging;

namespace CoreMemoryBus.Messages
{
    public static class SagaMessages
    {
        public class QuerySagaComplete : Message, ICorrelatedMessage
        {
            public QuerySagaComplete(Guid correlationId, IReplyEnvelope reply)
            {
                CorrelationId = correlationId;
                Reply = reply;
            }

            public Guid CorrelationId { get; private set; }

            public IReplyEnvelope Reply { get; private set; }
        }

        public class SagaCompleteReply : Message, ICorrelatedMessage
        {
            public SagaCompleteReply(Guid correlationId)
            {
                CorrelationId = correlationId;
            }

            public Guid CorrelationId { get; private set; }

            public bool IsComplete { get; set; }
        }

        public class DeleteSaga : Message, ICorrelatedMessage
        {
            public DeleteSaga(Guid correlationId)
            {
                CorrelationId = correlationId;
            }

            public Guid CorrelationId { get; private set; }
        }
    }
}