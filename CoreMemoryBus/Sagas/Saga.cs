using System;
using System.Collections.Generic;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Sagas
{
    public class Saga<TDerived> : ProxyPublisher<TDerived>
    {
        protected Saga()
        { }

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