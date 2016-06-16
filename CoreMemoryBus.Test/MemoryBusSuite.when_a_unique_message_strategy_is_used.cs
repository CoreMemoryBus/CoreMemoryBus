using System;
using CoreMemoryBus.Messages;
using CoreMemoryBus.PublishingStrategies;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    public partial class MemoryBusSuite
    {
        [TestFixture]
        public class when_a_unique_message_strategy_is_used
        {
            [Test]
            public void non_implementing_messages_can_be_published_more_than_once()
            {
                int executionCount = 0;
                var innerStrategy = new TestPublishingStrategy(() => executionCount++);
                var decoratedStrategy = new UniqueMessageStrategyDecorator(innerStrategy);
                decoratedStrategy.Publish(new TestMessage());
                decoratedStrategy.Publish(new TestMessage());

                Assert.That(executionCount, Is.EqualTo(2));
            }

            public class TestUniqueMessage : Messages.Message, IUniqueMessage
            {
                public TestUniqueMessage(Guid id)
                {
                    Id = id;
                }

                public Guid Id { get; private set; }
            }

            [Test]
            public void a_unique_message_can_only_be_published_once()
            {
                var guidFactory = CreateMockGuidFactory();
                var msg = new TestUniqueMessage(guidFactory.NewGuid());

                int executionCount = 0;
                var innerStrategy = new TestPublishingStrategy(() => executionCount++);
                var decoratedStrategy = new UniqueMessageStrategyDecorator(innerStrategy);
                decoratedStrategy.Publish(msg);
                decoratedStrategy.Publish(msg);

                Assert.That(executionCount, Is.EqualTo(1));
                Assert.That(decoratedStrategy.Count, Is.EqualTo(1));
            }
        }
    }
}