using System;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    /// <summary>
    /// An IRequestResponseHandler manages a message publication where a single response is expected. 
    /// As a reply message is received, it invokes a callback function which was nominated as the 
    /// request message was published. Both the request and reply messages must implement ICorrelatedMessage.
    /// The correlation Id is used to correlate a reply to a given request. 
    /// When a reply has been successfully called-back the reply handler will no longer activate with the same correlation Id.
    /// </summary>
    public interface IRequestResponseHandler<TCorrelation>
    {
        void Publish<TRequest, TResponse>(TRequest request, Action<TResponse> responseCallback)
            where TRequest : Message, ICorrelatedMessage<TCorrelation>
            where TResponse : Message, ICorrelatedMessage<TCorrelation>;

        void Await<TResponse>(TCorrelation correlationId, Action<TResponse> responseCallback)
            where TResponse : Message, ICorrelatedMessage<TCorrelation>;
    }
}