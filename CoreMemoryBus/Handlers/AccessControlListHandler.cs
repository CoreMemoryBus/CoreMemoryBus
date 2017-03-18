using System.Collections.Generic;
using System.Linq;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

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

        public void AddAccessControlList(IAccessControlList acl)
        {
            Ensure.ArgumentIsNotNull(acl, "acl");
            _acls.Add(acl);
        }

        public void Handle(AccessControlListMessages.Grant message)
        {
            _acls.ForEach(a => a.Grant(message.Principal, message.Type));
        }

        public void Handle(AccessControlListMessages.RevokeGrant message)
        {
            _acls.ForEach(a => a.RevokeGrant(message.Principal, message.Type));
        }

        public void Handle(AccessControlListMessages.Deny message)
        {
            _acls.ForEach(a => a.Deny(message.Principal, message.Type));
        }

        public void Handle(AccessControlListMessages.RevokeDeny message)
        {
            _acls.ForEach(a => a.RevokeDeny(message.Principal, message.Type));
        }

        public void Handle(AccessControlListMessages.InitialiseAccessControlList message)
        {
            message.AclCommands.ForEach(Publish);
        }

        public void Handle(AccessControlListMessages.RequestAccessControlExplanation message)
        {
            var firstAcl = _acls.FirstOrDefault();
            if (firstAcl != null)
            {
                var explanation = firstAcl.Explain(new[] {message.Principal}, message.Type);
                message.Reply.ReplyWith(
                    new AccessControlListMessages.AccessControlExplanationResponse(message.CorrelationId, explanation));
            }
        }
    }
}