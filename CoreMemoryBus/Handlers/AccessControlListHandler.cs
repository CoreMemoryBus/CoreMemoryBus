using System.Collections.Generic;
using System.Linq;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;
using System;
using static CoreMemoryBus.Messages.AccessControlListMessages;

namespace CoreMemoryBus.Handlers
{
    public class AccessControlListHandler : IHandle<Grant>,
                                            IHandle<RevokeGrant>,
                                            IHandle<Deny>,
                                            IHandle<RevokeDeny>,
                                            IHandle<InitialiseAccessControlList>,
                                            IHandle<RequestAccessControlExplanation>
    {
        private readonly List<IAccessControlList> _acls = new List<IAccessControlList>();
        private readonly IMessageSink _unpublishedMsgSink;

        public AccessControlListHandler(IMessageSink unpublishedMsgSink = null)
        {
            _unpublishedMsgSink = unpublishedMsgSink ?? new NullMessageSink();
        }

        public void AddAccessControlList(IAccessControlList acl)
        {
            Ensure.ArgumentIsNotNull(acl, "acl");
            _acls.Add(acl);
        }

        public void Handle(Grant message)
        {
            VerifyAcl(message, () =>
            {
                _acls.ForEach(a => a.Grant(message.ControlledMessageType, message.Principals));
            });
        }

        public void Handle(RevokeGrant message)
        {
            VerifyAcl(message, () =>
            {
                _acls.ForEach(a => a.RevokeGrant(message.ControlledMessageType, message.Principals));
            });
        }

        public void Handle(Deny message)
        {
            VerifyAcl(message, () =>
            {
                _acls.ForEach(a => a.Deny(message.ControlledMessageType, message.Principals));
            });
        }

        public void Handle(RevokeDeny message)
        {
            VerifyAcl(message, () =>
            {
                _acls.ForEach(a => a.RevokeDeny(message.ControlledMessageType, message.Principals));
            });
        }

        public void Handle(InitialiseAccessControlList message)
        {
            VerifyAcl(message, () =>
            {
                message.AclCommands.ForEach(Publish);
            });
        }

        #region Allow more efficient self publication

        private Dictionary<Type, IMessageHandlerProxy> handlers;

        private Dictionary<Type, IMessageHandlerProxy> Handlers { get { return handlers ?? (handlers = InitHandlers(this)); } }

        private static Dictionary<Type, IMessageHandlerProxy> InitHandlers(AccessControlListHandler host)
        {
            var handlers = new Dictionary<Type, IMessageHandlerProxy>
            {
                { typeof(Grant), new MessageHandlerProxy<Grant>(host) },
                { typeof(RevokeGrant), new MessageHandlerProxy<RevokeGrant>(host) },
                { typeof(Deny), new MessageHandlerProxy<Deny>(host) },
                { typeof(RevokeDeny), new MessageHandlerProxy<RevokeDeny>(host) },
                { typeof(InitialiseAccessControlList), new MessageHandlerProxy<InitialiseAccessControlList>(host) },
                { typeof(RequestAccessControlExplanation), new MessageHandlerProxy<RequestAccessControlExplanation>(host) },
            };
            return handlers;
        }

        #endregion

        private void Publish(Message message)
        {
            Handlers[message.GetType()].Publish(message);
        }

        public void Handle(RequestAccessControlExplanation message)
        {
            var firstAcl = _acls.FirstOrDefault();
            if (firstAcl != null)
            {
                var explanation = firstAcl.Explain(message.ControlledMessageType, message.Principals);
                message.Reply.ReplyWith(new AccessControlExplanation(message.CorrelationId, explanation));
            }
        }

        private void VerifyAcl(IAclAdminMessage adminMessage, Action successAction)
        {
            var firstAcl = _acls.FirstOrDefault();
            var aclMsgType = adminMessage.GetType();
            var aclPrincipals = adminMessage.AdminPrincipals;

            if (firstAcl.IsGranted(aclMsgType, aclPrincipals) &&
                !firstAcl.IsDenied(aclMsgType, aclPrincipals))
            {
                successAction();
            }
            else
            {
                _unpublishedMsgSink.ReceiveMessage(adminMessage as Message);
            }
        }
    }
}
