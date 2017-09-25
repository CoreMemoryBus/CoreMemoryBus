using System.Collections.Generic;
using System.Linq;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;
using System;

namespace CoreMemoryBus.Handlers
{
    public class AccessControlListHandler : ProxyPublisher,
                                            IHandle<AccessControlListMessages.Grant>,
                                            IHandle<AccessControlListMessages.RevokeGrant>,
                                            IHandle<AccessControlListMessages.Deny>,
                                            IHandle<AccessControlListMessages.RevokeDeny>,
                                            IHandle<AccessControlListMessages.InitialiseAccessControlList>,
                                            IHandle<AccessControlListMessages.RequestAccessControlExplanation>
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

        public void Handle(AccessControlListMessages.Grant message)
        {
            VerifyAcl(message, () =>
            {
                _acls.ForEach(a => a.Grant(message.ControlledMessageType, message.Principals));
            });
        }

        public void Handle(AccessControlListMessages.RevokeGrant message)
        {
            VerifyAcl(message, () =>
            {
                _acls.ForEach(a => a.RevokeGrant(message.ControlledMessageType, message.Principals));
            });
        }

        public void Handle(AccessControlListMessages.Deny message)
        {
            VerifyAcl(message, () =>
            {
                _acls.ForEach(a => a.Deny(message.ControlledMessageType, message.Principals));
            });
        }

        public void Handle(AccessControlListMessages.RevokeDeny message)
        {
            VerifyAcl(message, () =>
            {
                _acls.ForEach(a => a.RevokeDeny(message.ControlledMessageType, message.Principals));
            });
        }

        public void Handle(AccessControlListMessages.InitialiseAccessControlList message)
        {
            VerifyAcl(message, () =>
            {
                message.AclCommands.ForEach(Publish);
            });
        }

        public void Handle(AccessControlListMessages.RequestAccessControlExplanation message)
        {
            var firstAcl = _acls.FirstOrDefault();
            if (firstAcl != null)
            {
                var explanation = firstAcl.Explain(message.ControlledMessageType, message.Principals);
                message.Reply.ReplyWith(
                    new AccessControlListMessages.AccessControlExplanation(message.CorrelationId, explanation));
            }
        }

        private void VerifyAcl(IAclAdminMessage message, Action successAction)
        {
            var firstAcl = _acls.FirstOrDefault();
            var aclMsgType = message.GetType();
            var aclPrincipals = message.AdminPrincipals;

            if (firstAcl.IsGranted(aclMsgType, aclPrincipals) &&
                !firstAcl.IsDenied(aclMsgType, aclPrincipals))
            {
                successAction();
            }
        }
    }
}
