using System;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class CorrelatableRepository<THashKey, TRepoItem> : Repository<THashKey, TRepoItem>, IHandle<Message>
        where TRepoItem : IPublisher
    {
        protected CorrelatableRepository(Func<Message, TRepoItem> repoItemFactory = null) : base(repoItemFactory)
        { }

        public void Handle(Message msg)
        {
            var repoItemMessage = msg as ICorrelatedMessage<THashKey>;
            if (repoItemMessage != null)
            {
                TRepoItem repoItem;
                if (RepoItems.TryGetValue(repoItemMessage.CorrelationId, out repoItem))
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