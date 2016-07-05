using System;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public class ActionReplyEnvelope<T1> : IReplyEnvelope where T1 : Message
    {
        private readonly Action<T1> _action;

        public ActionReplyEnvelope(Action<T1> action)
        {
            _action = action;
        }

        public void ReplyWith<T>(T message) where T : Message
        {
            _action((T1)(Message)message);
        }
    }

    public static class ActionReplyEnvelope
    {
        public static ActionReplyEnvelope<T> New<T>(Action<T> action) where T:Message
        {
            return new ActionReplyEnvelope<T>(action);
        }
    }
}