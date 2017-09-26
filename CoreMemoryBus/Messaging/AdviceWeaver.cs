using System;
using CoreMemoryBus.Handlers;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public static class AdviceWeaver
    {
        public static IMessageHandlerProxy<T> AddBeforeAdvice<T>(this IHandle<T> handler, Action<T> beforeAdvice) where T : Message
        {
            return AddBeforeAdvice<T>(new MessageHandlerProxy<T>(handler), beforeAdvice);
        }

        public static IMessageHandlerProxy<T> AddBeforeAdvice<T>(this IMessageHandlerProxy<T> proxy, Action<T> beforeAdvice) where T : Message
        {
            return new BeforeAdviceHandlerProxy<T>(proxy, beforeAdvice);
        }

        public static IMessageHandlerProxy<T> AddAfterAdvice<T>(this IHandle<T> handler, Action<T> afterAdvice) where T : Message
        {
            return AddAfterAdvice(new MessageHandlerProxy<T>(handler), afterAdvice);
        }

        public static IMessageHandlerProxy<T> AddAfterAdvice<T>(this IMessageHandlerProxy<T> proxy, Action<T> afterAdvice) where T : Message
        {
            return new AfterAdviceHandlerProxy<T>(proxy, afterAdvice);
        }

        public static IMessageHandlerProxy<T> AddFinallyAdvice<T>(this IHandle<T> handler, Action<T> finallyAdvice) where T : Message
        {
            return AddFinallyAdvice(new MessageHandlerProxy<T>(handler), finallyAdvice);
        }

        public static IMessageHandlerProxy<T> AddFinallyAdvice<T>(this IMessageHandlerProxy<T> proxy, Action<T> finallyAdvice) where T : Message
        {
            return new FinallyAdviceHandlerProxy<T>(proxy, finallyAdvice);
        }

        #region Implementation

        internal class BeforeAdviceHandlerProxy<T> : IMessageHandlerProxy<T> where T : Message
        {
            private readonly IMessageHandlerProxy innerProxy;
            private readonly Action<T> beforeAdvice;

            public bool IsSame(object messageHandler)
            {
                return innerProxy.IsSame(messageHandler);
            }

            public void Publish(Message message)
            {
                beforeAdvice((T)message);
                innerProxy.Publish(message);
            }

            public BeforeAdviceHandlerProxy(IMessageHandlerProxy innerProxy, Action<T> beforeAdvice)
            {
                this.innerProxy = innerProxy;
                this.beforeAdvice = beforeAdvice;
            }
        }

        internal class AfterAdviceHandlerProxy<T> : IMessageHandlerProxy<T> where T : Message
        {
            private readonly IMessageHandlerProxy innerProxy;
            private readonly Action<T> afterAdvice;

            public bool IsSame(object messageHandler)
            {
                return innerProxy.IsSame(messageHandler);
            }

            public void Publish(Message message)
            {
                innerProxy.Publish(message);
                afterAdvice((T)message);
            }

            public AfterAdviceHandlerProxy(IMessageHandlerProxy innerProxy, Action<T> afterAdvice)
            {
                this.innerProxy = innerProxy;
                this.afterAdvice = afterAdvice;
            }
        }

        internal class FinallyAdviceHandlerProxy<T> : IMessageHandlerProxy<T> where T : Message
        {
            private readonly IMessageHandlerProxy innerProxy;
            private readonly Action<T> finallyAdvice;

            public bool IsSame(object messageHandler)
            {
                return innerProxy.IsSame(messageHandler);
            }

            public void Publish(Message message)
            {
                try
                {
                    innerProxy.Publish(message);
                }
                finally
                {
                    finallyAdvice((T)message);
                }
            }

            public FinallyAdviceHandlerProxy(IMessageHandlerProxy innerProxy, Action<T> finallyAdvice)
            {
                this.innerProxy = innerProxy;
                this.finallyAdvice = finallyAdvice;
            }
        }

        #endregion
    }
}
