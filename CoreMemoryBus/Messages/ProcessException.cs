using System;

namespace CoreMemoryBus.Messages
{
    public class ProcessException : Message, IUniqueMessage
    {
        public ProcessException(Guid id, Exception exception, Message message)
        {
            Id = id;
            Exception = exception;
            Message = message;
        }

        public bool IsHandled { get; set; }
        public Guid Id { get; private set; }
        public Exception Exception { get; private set; }
        public Message Message { get; private set; }
    }
}