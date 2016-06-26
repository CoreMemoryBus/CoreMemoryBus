using System;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace CoreMemoryBus.Test
{
    public class RequestReponseHandlerSuite : TestSuite
    {
        [TestFixture]
        public class when_publishing_with_a_request_response_handler
        {
            [Test]
            public void a_reply_handler_will_do_nothing_on_the_second_invocation()
            {
                var theMessageBus = new MemoryBus();
                var requestReponse = new RequestResponseHandler(theMessageBus);
                theMessageBus.Subscribe(requestReponse);

                var guid = Guid.NewGuid();

                theMessageBus.Subscribe(new CallerRequestHandler(theMessageBus));

                var responder = new CallerReponseHandler();

                //Invoke using the R/R handler instead of the bus.
                Action<CallerResponseMessage> callback = responder.ResponseCallback;
                requestReponse.Publish(new CallerRequestMessage(guid), callback);

                Assert.That(responder.InvocationCount, Is.EqualTo(1));

                // Try the same correlation again - it will do nothing this time.
                theMessageBus.Publish(new CallerRequestMessage(guid));
                Assert.That(responder.InvocationCount, Is.EqualTo(1));
            }
        }

        public RequestReponseHandlerSuite()
            : base("RequestReponseHandlerSuite")
        { }

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
            public int InvocationCount { get; private set; }
            public void ResponseCallback(CallerResponseMessage response)
            {
                ++InvocationCount;
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


    }
}