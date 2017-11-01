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
            public Principal Principal { get; private set; }
            public User AdminUser { get; private set; }

            protected AccessControlCommand(Guid id, Type msgType, User adminUser, Principal principal)
            {
                Id = id;
                ControlledMessageType = msgType;
                Principal = principal;
                AdminUser = adminUser;
            }
        }

        public class Grant : AccessControlCommand
        {
            public Grant(Guid id, Type msgType, User adminUser, Principal principal)
                : base(id, msgType, adminUser, principal)
            { }
        }

        public class RevokeGrant : AccessControlCommand
        {
            public RevokeGrant(Guid id, Type msgType, User adminUser, Principal principal)
                : base(id, msgType, adminUser, principal)
            { }
        }

        public class Deny : AccessControlCommand
        {
            public Deny(Guid id, Type msgType, User adminUser, Principal principal)
                : base(id, msgType, adminUser, principal)
            { }
        }

        public class RevokeDeny : AccessControlCommand
        {
            public RevokeDeny(Guid id, Type msgType, User adminUser, Principal principal)
                : base(id, msgType, adminUser, principal)
            { }
        }

        public class InitialiseAccessControlList : Message, IUniqueMessage, IAclAdminMessage
        {
            public readonly List<Message> AclCommands = new List<Message>();

            public InitialiseAccessControlList(Guid id, User adminUser)
            {
                Id = id;
                AdminUser = adminUser;
            }

            public Guid Id { get; private set; }

            public User AdminUser { get; }
        }

        public class RequestAccessControlExplanation : Message, ICorrelatedMessage<Guid>, IAccessControlMessage, IAclAdminMessage
        {
            public RequestAccessControlExplanation(Guid correlationId, IReplyEnvelope reply, Type type, User adminUser, Principal principal)
            {
                CorrelationId = correlationId;
                Reply = reply;
                ControlledMessageType = type;
                Principal = principal;
                AdminUser = adminUser;
            }

            public Guid CorrelationId { get; private set; }
            public IReplyEnvelope Reply { get; private set; }
            public Type ControlledMessageType { get; private set; }
            public Principal Principal { get; private set; }
            public User AdminUser { get; private set; }
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

        public static readonly Principal DefaultAdmin = new Principal { Name = "Admin", ReadOnly = true };

        public static void ApplyDefaultAdmin(this IAccessControlList acl)
        {
            acl.ApplyAdmin(DefaultAdmin);
        }

        public static void ApplyAdmin(this IAccessControlList acl, Principal defaultAdmin)
        {
            acl.Grant(typeof(Grant), defaultAdmin)
               .Grant(typeof(RevokeGrant), defaultAdmin)
               .Grant(typeof(Deny), defaultAdmin)
               .Grant(typeof(RevokeDeny), defaultAdmin)
               .Grant(typeof(InitialiseAccessControlList), defaultAdmin);
        }
    }
}
