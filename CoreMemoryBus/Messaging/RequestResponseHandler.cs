using System;
using System.Collections.Generic;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    /// <summary>
    /// A RequestResponseHandler manages a message publication where a single response is expected. 
    /// As a reply message is received, it invokes a callback function which was nominated as the 
    /// request message was published. Both the request and reply messages must implement ICorrelatedMessage.
    /// The correlation Id is used to correlate a reply to a given request. The reply handler can only be 
    /// invoked once. When a reply has been successfully called-back the reply handler will no longer 
    /// activate with the same correlation Id.
    /// </summary>
    public class RequestResponseHandler : IHandle<Message>
    {
        private readonly IPublisher _publisher;

        public RequestResponseHandler(IPublisher publisher)
        {
            _publisher = publisher;
        }

        readonly Dictionary<Guid, IResponseAction> _responseActions = new Dictionary<Guid, IResponseAction>();

        public void Publish<TRequest, TResponse>(TRequest request, Action<TResponse> responseCallback) 
            where TRequest : Message, ICorrelatedMessage
            where TResponse : Message, ICorrelatedMessage
        {
            _responseActions[request.CorrelationId] = new ResponseAction<TResponse>(responseCallback);
            _publisher.Publish(request);
        }

        interface IResponseAction
        {
            void ExecuteAction(Message replyMessage);
        }

        class ResponseAction<T> : IResponseAction where T : Message
        {
            private readonly Action<T> _action;

            public ResponseAction(Action<T> action)
            {
                _action = action;
            }

            public void ExecuteAction(Message replyMessage)
            {
                var param = (T) replyMessage;
                _action(param);
            }
        }

        public void Handle(Message message)
        {
            var reply = message as ICorrelatedMessage;
            if (reply != null)
            {
                IResponseAction action;
                if (_responseActions.TryGetValue(reply.CorrelationId, out action))
                {
                    action.ExecuteAction(message);
                    _responseActions.Remove(reply.CorrelationId);
                }
            }
        }
    }
}