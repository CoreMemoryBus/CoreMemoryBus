using System;
using CoreMemoryBus.Messaging;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    public partial class MemoryBusSuite
    {
        [TestFixture]
        public class when_subscribers_are_removed_from_a_message_bus
        {
            [Test]
            public void and_is_null_throws_a_ArgumentNullException()
            {
                var theMessageBus = new MemoryBus();
                Assert.Throws<ArgumentNullException>(() => theMessageBus.Unsubscribe<TestMessage>(null));
            }

            [Test]
            public void the_subscriber_no_longer_handles_messages()
            {
                bool isHandled = false;
                var theMessageBus = new MemoryBus();
                var handler1 = new TestMessageHandler<TestMessage>(msg => { isHandled = true; });
                theMessageBus.Subscribe(handler1);
                theMessageBus.Unsubscribe(handler1);

                theMessageBus.Publish(new TestMessage());
                Assert.That(isHandled, Is.False);
            }

            [Test]
            public void and_the_last_subscriber_is_removed_the_bus_no_longer_handles_the_message()
            {

            }
        }
    }
}