using System.Collections.Generic;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class MessageHandlerProxies : List<IMessageHandlerProxy>
    {
        public MessageHandlerProxies()
        { }

        public MessageHandlerProxies(MessageHandlerProxies rhs) : base(rhs)
        { }

        public void Publish(Message message)
        {
            this.ForEach(proxy => proxy.Publish(message));
        }
    }
}
