using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.PublishingStrategies
{
    /// <summary>
    /// AccessControlListPublishingStrategy is a decorator class that wraps around a 
    /// concrete publishing strategy (e.g. FlatPublishingStrategy,HeirarchicalPublishingStrategy). 
    /// This class facilitates access controlled publishing of an 
    /// IAccessControlledMessage, subject to the existence of grant-priveleges 
    /// or deny-priveleges in an AccessControlList. The Principals (user & 
    /// group membership) determine these rights for a given message type.
    /// Non-implementing messages are directly published.
    /// </summary>
    public class AccessControlListPublishingStrategy : IPublishingStrategy
    {
        private readonly IPublishingStrategy _implementation;
        private readonly IAccessControlList _acl;
        private readonly IMessageSink _unpublishedMsgSink;

        public AccessControlListPublishingStrategy(IPublishingStrategy implementation, IAccessControlList acl, IMessageSink unpublishedMsgSink)
        {
            _implementation = implementation;
            _acl = acl;
            _unpublishedMsgSink = unpublishedMsgSink;
        }

        public void Publish(Message message)
        {
            var aclMsg = message as IAccessControlledMessage;
            if (aclMsg != null)
            {
                var msgType = aclMsg.GetType();
                if (_acl.IsDenied(msgType, aclMsg.Principals) ||
                    !_acl.IsGranted(msgType, aclMsg.Principals))
                {
                    var explanation = _acl.Explain(msgType, aclMsg.Principals);
                    var unpublishedMsg = new AccessControlListMessages.NotPublishedMessage(explanation, message);
                    _unpublishedMsgSink.ReceiveMessage(unpublishedMsg);

                    return;
                }
            }

            _implementation.Publish(message);
        }
    }
}
