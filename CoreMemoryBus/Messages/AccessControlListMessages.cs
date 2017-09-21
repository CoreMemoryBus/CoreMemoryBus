using System;
using System.Collections.Generic;
using CoreMemoryBus.Messaging;

namespace CoreMemoryBus.Messages
{
    public static class AccessControlListMessages
    {
        public abstract class AccessControlCommand : Message, IAccessControlMessage, IUniqueMessage
        {
            public Guid Id { get; private set; }
            public Type Type { get; private set; }
            public string[] Principals { get; private set; }

            protected AccessControlCommand(Guid id, Type msgType, params string[] principals)
            {
                Id = id;
                Type = msgType;
                Principals = principals;
            }
        }

        public class Grant : AccessControlCommand
        {
            public Grant(Guid id, Type msgType, params string[] principals)
                : base(id, msgType, principals)
            { }
        }

        public class RevokeGrant : AccessControlCommand
        {
            public RevokeGrant(Guid id, Type msgType, params string[] principals)
                : base(id, msgType, principals)
            { }
        }

        public class Deny : AccessControlCommand
        {
            public Deny(Guid id, Type msgType, params string[] principals)
                : base(id, msgType, principals)
            { }
        }

        public class RevokeDeny : AccessControlCommand
        {
            public RevokeDeny(Guid id, Type msgType, params string[] principals)
                : base(id, msgType, principals)
            { }
        }

        public class InitialiseAccessControlList : Message, IUniqueMessage
        {
            public readonly List<AccessControlCommand> AclCommands = new List<AccessControlCommand>();

            public InitialiseAccessControlList(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; private set; }
        }

        public class RequestAccessControlExplanation : Message, ICorrelatedMessage<Guid>, IAccessControlMessage
        {
            public RequestAccessControlExplanation(Guid correlationId, IReplyEnvelope reply, Type type, params string[] principals)
            {
                CorrelationId = correlationId;
                Reply = reply;
                Type = type;
                Principals = principals;
            }

            public Guid CorrelationId { get; private set; }
            public IReplyEnvelope Reply { get; private set; }
            public Type Type { get; private set; }
            public string[] Principals { get; private set; }
        }

        public class AccessControlExplanation : Message, ICorrelatedMessage<Guid>
        {
            public AccessControlExplanation(Guid correlationId, string explanation)
            {
                CorrelationId = correlationId;
                Explanation = explanation;
            }

            public Guid CorrelationId { get; private set; }

            public string Explanation { get; private set; }
        }

        public class NotPublishedMessage : Message
        {
            public string Explanation { get; }
            public Message Message { get; }

            public NotPublishedMessage(string explanation, Message message)
            {
                Explanation = explanation;
                Message = message;
            }
        }
    }
}
