using System;
using System.Collections.Generic;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Messaging
{
    public class Repository<THashKey, TRepoItem, TRepoItemInterface>
        : IHandle<Message>
        where TRepoItem : TRepoItemInterface
        where TRepoItemInterface : IPublisher
    {
        private readonly Dictionary<THashKey, TRepoItemInterface> _repoItems = new Dictionary<THashKey, TRepoItemInterface>();

        protected Dictionary<THashKey, TRepoItemInterface> RepositoryItems { get { return _repoItems; } }

        private static readonly HashSet<Type> TriggerMessageTypes = new HashSet<Type>();

        static Repository()
        {
            var triggerHandlers = PubSubCommon.GetMessageTriggerInterfaces(typeof(TRepoItem).GetInterfaces());
            foreach (var trigger in triggerHandlers)
            {
                var triggerMsgType = trigger.GetGenericArguments()[0];
                TriggerMessageTypes.Add(triggerMsgType);
            }
        }

        public Repository(Func<Message, TRepoItemInterface> repoItemFactory = null)
        {
            RepoItemFactory = repoItemFactory ?? ((_) => (TRepoItemInterface)Activator.CreateInstance(typeof(TRepoItem)));
        }

        protected Func<Message, TRepoItemInterface> RepoItemFactory { get; private set; }

        public void Handle(Message msg)
        {
            var repoItemMessage = msg as ICorrelatedMessage<THashKey>;
            if (repoItemMessage != null)
            {
                TRepoItemInterface repoItem;
                if (_repoItems.TryGetValue(repoItemMessage.CorrelationId, out repoItem))
                {
                    repoItem.Publish(msg);
                }
                else if (TriggerMessageTypes.Contains(msg.GetType()))
                {
                    var newReadModel = RepoItemFactory(msg);
                    _repoItems[repoItemMessage.CorrelationId] = newReadModel;
                    newReadModel.Publish(msg);
                }
            }
        }
    }
}