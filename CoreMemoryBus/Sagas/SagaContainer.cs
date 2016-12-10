using System;
using System.Collections;
using System.Collections.Generic;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;

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
    /// 

    public class SagaContainer<TSaga> : Repository<Guid, TSaga, ISaga>, 
                                        IHandle<SagaMessages.QuerySagaComplete>,
                                        IHandle<SagaMessages.DeleteSaga>,
                                        IEnumerable<ISaga> where TSaga:ISaga
    {
        public int Count()
        {
            return RepositoryItems.Count;
        }

        public IEnumerator<ISaga> GetEnumerator()
        {
            return RepositoryItems.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Handle(SagaMessages.QuerySagaComplete message)
        {
            ISaga saga;
            if (RepositoryItems.TryGetValue(message.CorrelationId, out saga))
            {
                message.Reply.ReplyWith(new SagaMessages.SagaCompleteReply(message.CorrelationId) { IsComplete = saga.IsComplete });
            }
        }

        public void Handle(SagaMessages.DeleteSaga message)
        {
            RepositoryItems.Remove(message.CorrelationId);
        }
    }
}