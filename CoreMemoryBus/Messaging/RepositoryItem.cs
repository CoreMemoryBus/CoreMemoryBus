using System;
using System.Collections.Generic;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Messaging
{
    public class RepositoryItem<THashKey>
        : ProxyPublisher, ICorrelatable<THashKey>
    {
        protected RepositoryItem(THashKey correlationId)
        {
            CorrelationId = correlationId;
        }

        public THashKey CorrelationId { get; protected set; }

        protected override void CollectMessageHandlerProxies(Type[] interfaces, Dictionary<Type, IMessageHandlerProxy> result)
        {
            base.CollectMessageHandlerProxies(interfaces, result);
            InitTriggers(interfaces, result);
        }

        private void InitTriggers(Type[] interfaces, Dictionary<Type, IMessageHandlerProxy> result)
        {
            var triggers = PubSubCommon.GetMessageTriggerInterfaces(interfaces);
            foreach (var handler in triggers)
            {
                var msgType = handler.GetGenericArguments()[0];
                var proxyType = typeof(TriggerHandlerProxy<>).MakeGenericType(msgType);
                result.Add(msgType, (IMessageHandlerProxy)Activator.CreateInstance(proxyType, this));
            }
        }
    }
}
