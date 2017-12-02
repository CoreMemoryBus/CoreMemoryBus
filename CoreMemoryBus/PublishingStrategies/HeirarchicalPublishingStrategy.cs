using System;
using System.Collections.Generic;
using System.Text;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;

namespace CoreMemoryBus.PublishingStrategies
{
    /// <summary>
    /// HeirarchicalPublishingStrategy will publish to handlers of the message type and to all handlers of it's base classes. 
    /// This strategy should be used in most use cases. It makes possible the buses as handlers to other busses as well as other45
    /// polymorphic use cases.
    /// </summary>
    public class HeirarchicalPublishingStrategy : PublishingStrategy, IPublishingStrategy
    {
        public HeirarchicalPublishingStrategy(MessageHandlerDictionary messageHandlers, IMessageSink unhandledMessageSink = null)
            : base(messageHandlers)
        {
            UnhandledMessageSink = unhandledMessageSink ?? new NullMessageSink();
        }

        protected IMessageSink UnhandledMessageSink { get; }

        public void Publish(Message message)
        {
            var msgType = message.GetType();
            var messageWasHandled = TryPublish(message, msgType);
            do
            {
                msgType = msgType.BaseType;
                messageWasHandled |= TryPublish(message, msgType);
            }
            while (msgType != typeof(Message));

            if (!messageWasHandled)
            {
                UnhandledMessageSink.ReceiveMessage(message);
            }
        }
    }
}
