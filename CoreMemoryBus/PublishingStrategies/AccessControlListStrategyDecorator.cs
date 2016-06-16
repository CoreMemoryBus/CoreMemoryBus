using CoreMemoryBus.Logger;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.PublishingStrategies
{
    /// <summary>
    /// This class facilitates access controlled publishing of an 
    /// IAccessControlledMessage, subject to the existence of grant-priveleges 
    /// or deny-priveleges in an AccessControlList. The Principals (user & 
    /// group membership) determine these rights for a given message type.
    /// Non-implementing messages are directly published.
    /// </summary>
    public class AccessControlListStrategyDecorator : IPublishingStrategy
    {
        private readonly IPublishingStrategy _implementation;
        private readonly IAccessControlList _acl;
        private readonly ILogger _logger;

        public AccessControlListStrategyDecorator(IAccessControlList acl, ILogger logger, IPublishingStrategy implementation)
        {
            _implementation = implementation;
            _acl = acl;
            _logger = logger;
        }

        public void Publish(Message message)
        {
            var aclMsg = message as IAccessControlledMessage;
            if (aclMsg != null)
            {
                var msgType = aclMsg.GetType();
                if (_acl.IsDenied(aclMsg.Principals, msgType) ||
                    !_acl.IsGranted(aclMsg.Principals, msgType))
                {
                    var warning = _acl.Explain(aclMsg.Principals, msgType);
                    _logger.Warn(warning);

                    return;
                }
            }

            _implementation.Publish(message);
        }
    }
}
