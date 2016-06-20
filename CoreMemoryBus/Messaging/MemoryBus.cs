using System;
using System.Linq;
using CoreMemoryBus.Logger;
using CoreMemoryBus.Messages;
using CoreMemoryBus.PublishingStrategies;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.Messaging
{
    public class MemoryBus : IPublisher, ISubscriber, IHandle<Message>
    {
        protected ILogger Logger { get; private set; }

        public string Name { get; set; }
        public Guid Id { get; set; }

        public MemoryBus(ILogger logger = null, Func<MessageHandlerDictionary, IPublishingStrategy> publishingStrategyFactory = null)
        {
            Logger = logger ?? new NullLogger();
            Name = string.Empty;
            Id = Guid.Empty;

            _messageHandlers = new MessageHandlerDictionary();
            _publishingStrategy = publishingStrategyFactory == null 
                ? new HeirarchicalPublishingStrategy(_messageHandlers) 
                : publishingStrategyFactory(_messageHandlers);
        }

        public void Publish(Message message)
        {
            Ensure.ArgumentIsNotNull(message, "message");

            _publishingStrategy.Publish(message);
        }

        private readonly MessageHandlerDictionary _messageHandlers;
        private readonly IPublishingStrategy _publishingStrategy;

        public void Subscribe<T>(IHandle<T> messageHandler) where T : Message
        {
            Ensure.ArgumentIsNotNull(messageHandler, "messageHandler");

            MessageHandlerProxies proxies;
            if (!_messageHandlers.TryGetValue(typeof(T), out proxies))
            {
                proxies = new MessageHandlerProxies();
                _messageHandlers.Add(typeof(T), proxies);
            }

            if (!proxies.Any(p => p.IsSame(messageHandler)))
            {
                proxies.Add(new MessageHandlerProxy<T>(messageHandler));
            }
        }

        public void Subscribe(object messageHandler)
        {
            Ensure.ArgumentIsNotNull(messageHandler, "messageHandler");

            var handlerTypes = PubSubCommon.GetMessageHandlerInterfaces(messageHandler.GetType().GetInterfaces());
            foreach (var handlerType in handlerTypes)
            {
                var msgType = handlerType.GetGenericArguments()[0];
                var proxyType = typeof(MessageHandlerProxy<>).MakeGenericType(msgType);
                var handlerProxy = (IMessageHandlerProxy)Activator.CreateInstance(proxyType, messageHandler);

                MessageHandlerProxies proxies;
                if (!_messageHandlers.TryGetValue(msgType, out proxies))
                {
                    proxies = new MessageHandlerProxies();
                    _messageHandlers.Add(msgType, proxies);
                }

                if (!proxies.Any(p => p.IsSame(messageHandler)))
                {
                    proxies.Add(handlerProxy);
                }
            }
        }

        public void Unsubscribe<T>(IHandle<T> messageHandler) where T : Message
        {
            Ensure.ArgumentIsNotNull(messageHandler, "messageHandler");

            var msgType = typeof (T);
            MessageHandlerProxies proxies;
            if (_messageHandlers.TryGetValue(msgType, out proxies))
            {
                var proxy = proxies.FirstOrDefault(p => p.IsSame(messageHandler));
                if (proxy != null)
                {
                    proxies.Remove(proxy);
                    if (proxies.Count == 0)
                    {
                        _messageHandlers.Remove(msgType);
                    }
                }
            }
        }

        public void Unsubscribe(object messageHandler)
        {
            Ensure.ArgumentIsNotNull(messageHandler, "messageHandler");

            var handlerTypes = PubSubCommon.GetMessageHandlerInterfaces(messageHandler.GetType().GetInterfaces());
            foreach (var handlerType in handlerTypes)
            {
                var msgType = handlerType.GetGenericArguments()[0];
                MessageHandlerProxies proxies;
                if (_messageHandlers.TryGetValue(msgType, out proxies))
                {
                    var proxy = proxies.FirstOrDefault(p => p.IsSame(messageHandler));
                    if (proxy != null)
                    {
                        proxies.Remove(proxy);
                        if (proxies.Count == 0)
                        {
                            _messageHandlers.Remove(msgType);
                        }
                    }
                }
            }
        }

        public void Handle(Message message)
        {
            Publish(message);
        }
    }
}