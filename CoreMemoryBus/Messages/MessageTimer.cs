using System;
using CoreMemoryBus.Messages;

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
    }
}