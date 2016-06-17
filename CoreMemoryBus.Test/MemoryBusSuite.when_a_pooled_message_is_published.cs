using CoreMemoryBus.DataStructures;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.PublishingStrategies;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    public partial class MemoryBusSuite
    {
        [TestFixture]
        public class when_a_pooled_message_is_published
        {
            public class TestPublishingStrategy : IPublishingStrategy
            {
                public void Publish(Message message) {}
            }

            public class PooledMessage : Message, IPooledMessage
            {
                private readonly ObjectPool<PooledMessage> _owner;

                internal PooledMessage(ObjectPool<PooledMessage> owner)
                {
                    _owner = owner;
                }

                public void Release()
                {
                    _owner.Return(this);
                }
            }

            public class TestMessagePool : ObjectPool<PooledMessage>
            {
                public TestMessagePool(int maxCapacity) : 
                    base(maxCapacity, owner => new PooledMessage(owner))
                {}
            }

            [Test]
            public void it_is_returned_to_the_object_pool_afterwards()
            {
                var messagePool = new TestMessagePool(1);
                var strategy = new TestPublishingStrategy();
                var decoratedStrategy = new PooledMessageStrategyDecorator(strategy);

                var newMessage = messagePool.Borrow();
                // The pool should now be empty and will throw if we borrow again
                Assert.Throws<MaxCapacityException>(() => messagePool.Borrow());
                Assert.That(messagePool.Count, Is.EqualTo(0));

                decoratedStrategy.Publish(newMessage);

                Assert.That(messagePool.Count, Is.EqualTo(1));
            }
        }
    }
}