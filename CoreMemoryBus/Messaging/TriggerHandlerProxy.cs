using CoreMemoryBus.Handlers;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class TriggerHandlerProxy<T> : IMessageHandlerProxy where T : Message
    {
        private readonly IAmTriggeredBy<T> _trigger;

        public bool TryHandle(Message message)
        {
            var msg = (T)message;
            _trigger.Handle(msg);
            return true;
        }

        public TriggerHandlerProxy(IAmTriggeredBy<T> trigger)
        {
            _trigger = trigger;
        }

        public void Publish(Message message)
        {
            _trigger.Handle((T)message);
        }

        public bool IsSame(object trigger)
        {
            return ReferenceEquals(_trigger, trigger);
        }
    }
}
