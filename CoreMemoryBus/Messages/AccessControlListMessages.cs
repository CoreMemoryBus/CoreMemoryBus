﻿using System;
using System.Collections.Generic;
using CoreMemoryBus.Messaging;

namespace CoreMemoryBus.Messages
{
    public static class AccessControlListMessages
    {
        public abstract class AccessControlCommand : Message, IAccessControlMessage, IUniqueMessage
        {
            public Guid Id { get; private set; }
            public string Principal { get; private set; }
            public Type Type { get; private set; }

            protected AccessControlCommand(Guid id, string principal, Type msgType)
            {
                Id = id;
                Principal = principal;
                Type = msgType;
            }
        }

        public class Grant : AccessControlCommand
        {
            public Grant(Guid id, string principal, Type msgType)
                : base(id, principal, msgType)
            { }
        }

        public class RevokeGrant : AccessControlCommand
        {
            public RevokeGrant(Guid id, string principal, Type msgType)
                : base(id, principal, msgType)
            { }
        }

        public class Deny : AccessControlCommand
        {
            public Deny(Guid id, string principal, Type msgType)
                : base(id, principal, msgType)
            { }
        }

        public class RevokeDeny : AccessControlCommand
        {
            public RevokeDeny(Guid id, string principal, Type msgType)
                : base(id, principal, msgType)
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
            public RequestAccessControlExplanation(Guid correlationId, string principal, Type type, IReplyEnvelope reply)
            {
                CorrelationId = correlationId;
                Principal = principal;
                Type = type;
                Reply = reply;
            }

            public Guid CorrelationId { get; private set; }
            public string Principal { get; private set; }
            public Type Type { get; private set; }

            public IReplyEnvelope Reply { get; private set; }
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
            private Message _message;

            public NotPublishedMessage(Message message)
            {
                _message = message;
            }
        }
    }
}
