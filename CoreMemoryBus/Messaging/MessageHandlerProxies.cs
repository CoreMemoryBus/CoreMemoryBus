using System.Collections.Generic;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class MessageHandlerProxies : HashSet<IMessageHandlerProxy>
    {
        public MessageHandlerProxies()
        { }

        public MessageHandlerProxies(MessageHandlerProxies rhs) : base(rhs)
        { }

        public void Publish(Message message)
        {
            foreach(var proxy in this)
            {
                proxy.Publish(message);
            }
        }
    }
}
