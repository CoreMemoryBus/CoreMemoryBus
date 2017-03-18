using System;
using System.Collections.Generic;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Sagas
{
    /// <summary>
    /// A Saga is used to handle events on workflows requiring multiple messages to bring the workflow to completion.
    /// The saga is implemented by implementing at least 1 trigger via the IAmTriggeredBy interface and a succession of 
    /// conventional message handlers via the IHandleInterface. Saga objects are contained within a SagaContainer which 
    /// regulates their lifecycle. 
    /// </summary>
    public class Saga : ProxyPublisher
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