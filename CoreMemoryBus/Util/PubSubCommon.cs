using CoreMemoryBus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreMemoryBus.Util
{
    public static class PubSubCommon
    {
        public static IEnumerable<Type> GetMessageHandlerInterfaces(Type[] interfaces)
        {
            return from i in interfaces
                   where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandle<>)
                   select i;
        }
        public static IEnumerable<Type> GetMessageTriggerInterfaces(Type[] interfaces)
        {
            return from i in interfaces
                   where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAmTriggeredBy<>)
                   select i;
        }
    }
}
