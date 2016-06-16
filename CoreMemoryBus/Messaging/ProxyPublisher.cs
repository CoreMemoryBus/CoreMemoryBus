using System;
using System.Collections.Generic;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Messaging
{
    public class ProxyPublisher<TDerived> : IPublisher
    {
        protected ProxyPublisher()
        { }

        Dictionary<Type, IMessageHandlerProxy> _proxies;

        public void Publish(Messages.Message msg)
        {
            if (_proxies == null)
            {
                _proxies = InitProxies();
            }

            IMessageHandlerProxy proxy;
            if (_proxies.TryGetValue(msg.GetType(), out proxy))
            {
                proxy.Publish(msg);
            }
        }

        private Dictionary<Type, IMessageHandlerProxy> InitProxies()
        {
            var interfaces = typeof(TDerived).GetInterfaces();
            var result = new Dictionary<Type, IMessageHandlerProxy>();
            CollectMessageHandlerProxies(interfaces, result);
            return result;
        }

        protected virtual void CollectMessageHandlerProxies(Type[] interfaces, Dictionary<Type, IMessageHandlerProxy> result)
        {
            InitProxies(interfaces, result);
        }

        private void InitProxies(Type[] interfaces, Dictionary<Type, IMessageHandlerProxy> result)
        {
            var handlerTypes = PubSubCommon.GetMessageHandlerInterfaces(interfaces);
            foreach (var handlerType in handlerTypes)
            {
                var msgType = handlerType.GetGenericArguments()[0];
                var proxyType = typeof(MessageHandlerProxy<>).MakeGenericType(msgType);
                var handlerProxy = (IMessageHandlerProxy) Activator.CreateInstance(proxyType, this);
                result.Add(msgType, handlerProxy);
            }
        }
    }
}