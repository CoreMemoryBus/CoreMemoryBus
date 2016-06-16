using System;
using CoreMemoryBus.Messaging;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    public partial class MemoryBusSuite
    {
        [TestFixture]
        public class when_a_message_is_published_on_a_message_bus
        {
            [Test]
            public void and_is_null_throws_a_ArgumentNullException()
            {
                var theMessageBus = new MemoryBus();
                Assert.Throws<ArgumentNullException>(() => theMessageBus.Publish((TestMessage)null));
            }

            public class OtherMessage : Messages.Message { }

            [Test]
            public void a_message_handler_will_not_respond_if_it_does_not_handle_the_message()
            {
                bool isHandled = false;
                var theMessageBus = new MemoryBus();
                var handler1 = new TestMessageHandler<TestMessage>(msg => { isHandled = true; });
                theMessageBus.Subscribe(handler1);

                theMessageBus.Publish(new OtherMessage());

                Assert.That(isHandled, Is.False);
            }

            [Test]
            public void all_subscribers_for_the_message_handle_the_same_message()
            {
                var theMessageBus = new MemoryBus();
                var handler1 = new TestMessageHandler<TestMessage>(msg => { msg.IsHandledOne = true; });
                var handler2 = new TestMessageHandler<TestMessage>(msg => { msg.IsHandledTwo = true; });
                theMessageBus.Subscribe(handler1);
                theMessageBus.Subscribe(handler2);

                var m = new TestMessage { IsHandledOne = false, IsHandledTwo = false };
                theMessageBus.Publish(m);
                Assert.That(m.IsHandledOne, Is.True);
                Assert.That(m.IsHandledTwo, Is.True);
            }

            [Test]
            public void all_subscribers_handle_the_message_once_only()
            {
                var theMessageBus = new MemoryBus();
                var handler = new TestMessageHandler<TestMessage>(msg => { ++msg.HandlingCount; });
                theMessageBus.Subscribe(handler);
                theMessageBus.Subscribe(handler);

                var m = new TestMessage();
                theMessageBus.Publish(m);
                Assert.That(m.HandlingCount, Is.EqualTo(1));
            }

            public class BaseMessage : Messages.Message { public bool BaseHandled { get; set; } }

            public class DerivedMessage : BaseMessage { public bool DerivedHandled { get; set; } }

            [Test]
            public void all_subscribers_for_the_message_and_base_message_type_handle_the_same_message()
            {
                var theMessageBus = new MemoryBus();
                var derivedHandler = new TestMessageHandler<DerivedMessage>(msg => { msg.DerivedHandled = true; });
                theMessageBus.Subscribe(derivedHandler);
                var baseHandler = new TestMessageHandler<BaseMessage>(msg => { msg.BaseHandled = true; });
                theMessageBus.Subscribe(baseHandler);

                var testMessage = new DerivedMessage();
                theMessageBus.Publish(testMessage);

                Assert.That(testMessage.DerivedHandled, Is.True);
                Assert.That(testMessage.BaseHandled, Is.True);
            }

            [Test]
            public void and_is_not_handled_by_the_bus_it_is_forwarded_to_another_bus()
            {
                var rootBus = new MemoryBus { Name = "Root Bus" };

                bool isHandledByChildBus = false;
                var childBus = new MemoryBus { Name = "Child Bus" };
                var otherHandler = new TestMessageHandler<OtherMessage>(msg => { isHandledByChildBus = true; });
                childBus.Subscribe(otherHandler);

                rootBus.Subscribe(childBus);

                rootBus.Publish(new OtherMessage());

                Assert.That(isHandledByChildBus, Is.True);
            }
        }
    }
}