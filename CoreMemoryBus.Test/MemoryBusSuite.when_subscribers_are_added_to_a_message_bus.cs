using System;
using CoreMemoryBus.Messaging;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    public partial class MemoryBusSuite
    {
        [TestFixture]
        public class when_subscribers_are_added_to_a_message_bus
        {
            [Test]
            public void and_is_null_throws_a_ArgumentNullException()
            {
                var theMessageBus = new MemoryBus();
                Assert.Throws<ArgumentNullException>(() => theMessageBus.Subscribe<TestMessage>(null));
            }
        }
    }
}
