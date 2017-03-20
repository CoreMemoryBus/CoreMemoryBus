using System;
using System.Collections;
using System.Collections.Generic;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Messaging
{
    public class Repository<THashKey, TRepoItem> : IEnumerable<KeyValuePair<THashKey, TRepoItem>>
    {
        protected readonly Dictionary<THashKey, TRepoItem> RepoItems = new Dictionary<THashKey, TRepoItem>();

        protected static readonly HashSet<Type> TriggerMessageTypes = new HashSet<Type>();

        static Repository()
        {
            var triggerHandlers = PubSubCommon.GetMessageTriggerInterfaces(typeof(TRepoItem).GetInterfaces());
            foreach (var trigger in triggerHandlers)
            {
                var triggerMsgType = trigger.GetGenericArguments()[0];
                TriggerMessageTypes.Add(triggerMsgType);
            }
        }

        protected Repository(Func<Message, TRepoItem> repoItemFactory = null)
        {
            RepoItemFactory = repoItemFactory ?? (_ => (TRepoItem)Activator.CreateInstance(typeof(TRepoItem)));
        }

        protected Func<Message, TRepoItem> RepoItemFactory { get; set; }

        public TRepoItem Remove(THashKey key)
        {
            TRepoItem item;
            if (RepoItems.TryGetValue(key, out item))
            {
                RepoItems.Remove(key);
                return item;
            }

            return default(TRepoItem);
        }

        public IEnumerable<KeyValuePair<THashKey, TRepoItem>> Remove(ISet<THashKey> keys)
        {
            var result = new List<KeyValuePair<THashKey, TRepoItem>>();
            foreach (var hashKey in keys)
            {
                TRepoItem item;
                if (RepoItems.TryGetValue(hashKey, out item))
                {
                    result.Add(new KeyValuePair<THashKey, TRepoItem>(hashKey, item));
                }
            }

            return result;
        }

        public IEnumerator<KeyValuePair<THashKey, TRepoItem>> GetEnumerator()
        {
            return RepoItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}