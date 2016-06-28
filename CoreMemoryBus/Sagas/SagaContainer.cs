using System;
using System.Collections;
using System.Collections.Generic;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Sagas
{
    /// <summary>
    /// This class is responsible for managing the lifecycle of the Saga objects that it creates, as well as
    /// being used to route messages from a MemoryBus into a specific Saga. Only messages implementing 
    /// ICorrelatedMessage will be handled.
    /// If a Saga has dependencies and cannot be constructed using a default constructor, a custom Saga 
    /// object factory should be provided in the constructor.
    /// </summary>
    /// <typeparam name="TSaga"></typeparam>
    public class SagaContainer<TSaga> : IHandle<Message>, IEnumerable<ISaga>
        where TSaga : ISaga
    {
        private readonly Dictionary<Guid, ISaga> _sagaInstances = new Dictionary<Guid, ISaga>();

        private static readonly HashSet<Type> TriggerMessageTypes = new HashSet<Type>(); 

        static SagaContainer()
        {
            var triggerHandlers = PubSubCommon.GetMessageTriggerInterfaces(typeof (TSaga).GetInterfaces());
            foreach (var trigger in triggerHandlers)
            {
                var triggerMsgType = trigger.GetGenericArguments()[0];
                TriggerMessageTypes.Add(triggerMsgType);
            }
        }

        public SagaContainer(Func<Message, ISaga> sagaFactory = null)
        {
            SagaFactory = sagaFactory ?? ((_) => (ISaga) Activator.CreateInstance(typeof (TSaga)));
        }

        protected Func<Message, ISaga> SagaFactory { get; private set; }

        public void Handle(Message message)
        {
            var sagaMessage = message as ICorrelatedMessage;
            if (sagaMessage != null)
            {
                ISaga saga;
                if (_sagaInstances.TryGetValue(sagaMessage.CorrelationId, out saga))
                {
                    saga.Publish(message);
                }
                else if (TriggerMessageTypes.Contains(message.GetType()))
                {
                    var newSaga = SagaFactory(message);
                    _sagaInstances[sagaMessage.CorrelationId] = newSaga;
                    newSaga.Publish(message);
                }
            }
        }

        public int Count()
        {
            return _sagaInstances.Count;
        }

        public IEnumerator<ISaga> GetEnumerator()
        {
            return _sagaInstances.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}