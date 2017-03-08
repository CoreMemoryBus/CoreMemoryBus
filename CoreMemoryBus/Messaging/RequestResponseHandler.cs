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
    public class RequestResponseHandler<TCorrelation> : IHandle<Message>
    {
        private readonly Dictionary<TCorrelation, IResponseAction> _responseActions = new Dictionary<TCorrelation, IResponseAction>();
        private readonly IPublisher _publisher;

        public RequestResponseHandler(IPublisher publisher)
        {
            this._publisher = publisher;
        }

        public void Publish<TRequest, TResponse>(TRequest request, Action<TResponse> responseCallback)
            where TRequest : Message, ICorrelatedMessage<TCorrelation>
            where TResponse : Message, ICorrelatedMessage<TCorrelation>
        {
            _responseActions[request.CorrelationId] = new ResponseAction<TResponse>(responseCallback);
            _publisher.Publish(request);
        }

        public void Handle(Message message)
        {
            var correlatedMessage = message as ICorrelatedMessage<TCorrelation>;
            IResponseAction responseAction;
            if (correlatedMessage == null || !_responseActions.TryGetValue(correlatedMessage.CorrelationId, out responseAction))
            {
                return;
            }

            responseAction.ExecuteAction(message);
            _responseActions.Remove(correlatedMessage.CorrelationId);
        }

        private interface IResponseAction
        {
            void ExecuteAction(Message replyMessage);
        }

        private class ResponseAction<TMessage> : IResponseAction where TMessage : Message
        {
            private readonly Action<TMessage> _action;

            public ResponseAction(Action<TMessage> action)
            {
                _action = action;
            }

            public void ExecuteAction(Message replyMessage)
            {
                _action((TMessage)replyMessage);
            }
        }
    }
}