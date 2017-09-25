using System;
using CoreMemoryBus.Handlers;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public static class AdviceWeaver
    {
        public static IMessageHandlerProxy CreateBeforeAdvice<T>(IHandle<T> handler, Action<T> beforeAdvice) where T : Message
        {
            return AddBeforeAdvice<T>(new MessageHandlerProxy<T>(handler), beforeAdvice);
        }

        public static IMessageHandlerProxy AddBeforeAdvice<T>(IMessageHandlerProxy proxy, Action<T> beforeAdvice) where T : Message
        {
            return new BeforeAdviceHandlerProxy<T>(proxy, beforeAdvice);
        }

        public static IMessageHandlerProxy CreateAfterAdvice<T>(IHandle<T> handler, Action<T> afterAdvice) where T : Message
        {
            return AddAfterAdvice(new MessageHandlerProxy<T>(handler), afterAdvice);
        }

        public static IMessageHandlerProxy AddAfterAdvice<T>(IMessageHandlerProxy proxy, Action<T> afterAdvice) where T : Message
        {
            return new AfterAdviceHandlerProxy<T>(proxy, afterAdvice);
        }

        public static IMessageHandlerProxy CreateFinallyAdvice<T>(IHandle<T> handler, Action<T> finallyAdvice) where T : Message
        {
            return AddFinallyAdvice(new MessageHandlerProxy<T>(handler), finallyAdvice);
        }

        public static IMessageHandlerProxy AddFinallyAdvice<T>(IMessageHandlerProxy proxy, Action<T> finallyAdvice) where T : Message
        {
            return new FinallyAdviceHandlerProxy<T>(proxy, finallyAdvice);
        }

        internal class BeforeAdviceHandlerProxy<T> : IMessageHandlerProxy where T : Message
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

        internal class AfterAdviceHandlerProxy<T> : IMessageHandlerProxy where T : Message
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

        internal class FinallyAdviceHandlerProxy<T> : IMessageHandlerProxy where T : Message
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
    }
}
