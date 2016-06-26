using System;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    [TestFixture]
    public partial class RequestReponseHandlerSuite
    {
        class CallerRequestMessage : Message, ICorrelatedMessage
        {
            public CallerRequestMessage(Guid correlationId)
            {
                CorrelationId = correlationId;
            }

            public Guid CorrelationId { get; private set; }
        }

        class CallerResponseMessage : Message, ICorrelatedMessage
        {
            public CallerResponseMessage(Guid correlationId)
            {
                CorrelationId = correlationId;
            }

            public Guid CorrelationId { get; private set; }
        }

        class CallerReponseHandler
        {
            public bool IsInvoked { get; private set; }
            public void ResponseCallback(CallerResponseMessage response)
            {
                IsInvoked = true;
            }
        }

        class CallerRequestHandler : IHandle<CallerRequestMessage>
        {
            private readonly IPublisher _publisher;

            public CallerRequestHandler(IPublisher publisher)
            {
                _publisher = publisher;
            }

            public void Handle(CallerRequestMessage requestMessage)
            {
                // Return the result of a query or return information in the response

                _publisher.Publish(new CallerResponseMessage(requestMessage.CorrelationId));
            }
        }

        [Test]
        public void TestRequestReponseHandler()
        {
            var theMessageBus = new MemoryBus();
            var requestReponse = new RequestResponseHandler(theMessageBus);
            theMessageBus.Subscribe(requestReponse);

            var guid = Guid.NewGuid();

            theMessageBus.Subscribe(new CallerRequestHandler(theMessageBus));

            var responder = new CallerReponseHandler();

            Action<CallerResponseMessage> callback = responder.ResponseCallback;
            requestReponse.Publish(new CallerRequestMessage(guid), callback);

            Assert.That(responder.IsInvoked, Is.True);
        }        
    }
}