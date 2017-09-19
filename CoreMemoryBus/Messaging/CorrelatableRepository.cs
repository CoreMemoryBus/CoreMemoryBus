using System;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Handlers;

namespace CoreMemoryBus.Messaging
{
    public class CorrelatableRepository<THashKey, TRepoItem> : Repository<THashKey, TRepoItem>, IHandle<Message>
        where TRepoItem : IPublisher
    {
        protected CorrelatableRepository(Func<Message, TRepoItem> repoItemFactory = null) : base(repoItemFactory)
        { }

        public void Handle(Message msg)
        {
            if (msg is ICorrelatedMessage<THashKey> repoItemMessage)
            {
                if (RepoItems.TryGetValue(repoItemMessage.CorrelationId, out TRepoItem repoItem))
                {
                    repoItem.Publish(msg);
                }
                else if (TriggerMessageTypes.Contains(msg.GetType()))
                {
                    var newRepoItem = RepoItemFactory(msg);
                    RepoItems[repoItemMessage.CorrelationId] = newRepoItem;
                    newRepoItem.Publish(msg);
                }
            }
        }
    }
}