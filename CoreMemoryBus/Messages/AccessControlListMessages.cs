using System;
using System.Collections.Generic;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Messages
{
    public static class AccessControlListMessages
    {
        public abstract class AccessControlCommand : Message, IAccessControlMessage, IUniqueMessage
        {
            public Guid Id { get; private set; }
            public Type ControlledMessageType { get; private set; }
            public string[] Principals { get; private set; }
            public string[] AdminPrincipals { get; private set; }

            protected AccessControlCommand(Guid id, Type msgType, string[] adminPrincipals, string[] principals)
            {
                Id = id;
                ControlledMessageType = msgType;
                Principals = principals;
                AdminPrincipals = adminPrincipals;
            }
        }

        public class Grant : AccessControlCommand
        {
            public Grant(Guid id, Type msgType, string[] adminPrincipals, string[] principals)
                : base(id, msgType, adminPrincipals, principals)
            { }
        }

        public class RevokeGrant : AccessControlCommand
        {
            public RevokeGrant(Guid id, Type msgType, string[] adminPrincipals, string[] principals)
                : base(id, msgType, adminPrincipals, principals)
            { }
        }

        public class Deny : AccessControlCommand
        {
            public Deny(Guid id, Type msgType, string[] adminPrincipals, string[] principals)
                : base(id, msgType, adminPrincipals, principals)
            { }
        }

        public class RevokeDeny : AccessControlCommand
        {
            public RevokeDeny(Guid id, Type msgType, string[] adminPrincipals, string[] principals)
                : base(id, msgType, adminPrincipals, principals)
            { }
        }

        public class InitialiseAccessControlList : Message, IUniqueMessage, IAclAdminMessage
        {
            public readonly List<Message> AclCommands = new List<Message>();

            public InitialiseAccessControlList(Guid id, string[] adminPrincipals)
            {
                Id = id;
                AdminPrincipals = adminPrincipals;
            }

            public Guid Id { get; private set; }

            public string[] AdminPrincipals { get; }
        }

        public class RequestAccessControlExplanation : Message, ICorrelatedMessage<Guid>, IAccessControlMessage, IAclAdminMessage
        {
            public RequestAccessControlExplanation(Guid correlationId, IReplyEnvelope reply, Type type, string[] adminPrincipals, string[] principals)
            {
                CorrelationId = correlationId;
                Reply = reply;
                ControlledMessageType = type;
                Principals = principals;
                AdminPrincipals = adminPrincipals;
            }

            public Guid CorrelationId { get; private set; }
            public IReplyEnvelope Reply { get; private set; }
            public Type ControlledMessageType { get; private set; }
            public string[] Principals { get; private set; }
            public string[] AdminPrincipals { get; private set; }
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

        public const string DefaultAdmin = "Admin";

        public static void ApplyAdmin(this IAccessControlList acl, string defaultAdmin = DefaultAdmin)
        {
            acl.Grant(typeof(Grant), defaultAdmin)
               .Grant(typeof(RevokeGrant), defaultAdmin)
               .Grant(typeof(Deny), defaultAdmin)
               .Grant(typeof(RevokeDeny), defaultAdmin)
               .Grant(typeof(InitialiseAccessControlList), defaultAdmin);
        }
    }
}
