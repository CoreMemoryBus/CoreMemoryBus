using System;
using System.Collections.Generic;
using CoreMemoryBus.Handlers;
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
    public class RequestResponseHandler<TCorrelation> : IHandle<Message>, IRequestResponseHandler<TCorrelation>
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
            Await(request.CorrelationId, responseCallback);
            _publisher.Publish(request);
        }

        public void Await<TResponse>(TCorrelation correlationId, Action<TResponse> responseCallback) where TResponse : Message, ICorrelatedMessage<TCorrelation>
        {
            _responseActions[correlationId] = new ResponseAction<TResponse>(responseCallback);
        }

        public void Handle(Message message)
        {
            var correlatedMessage = message as ICorrelatedMessage<TCorrelation>;
            if (correlatedMessage == null || !_responseActions.TryGetValue(correlatedMessage.CorrelationId, out IResponseAction responseAction))
            {
                return;
            }

            if (responseAction.TryExecuteResponse(message))
            {
                _responseActions.Remove(correlatedMessage.CorrelationId);
            }
        }

        private interface IResponseAction
        {
            bool TryExecuteResponse(Message replyMessage);
        }

        private class ResponseAction<TMessage> : IResponseAction where TMessage : Message
        {
            private readonly Action<TMessage> _action;

            public ResponseAction(Action<TMessage> action)
            {
                _action = action;
            }

            public bool TryExecuteResponse(Message replyMessage)
            {
                if (replyMessage is TMessage specificReply)
                {
                    _action(specificReply);
                    return true;
                }

                return false;
            }
        }
    }
}
