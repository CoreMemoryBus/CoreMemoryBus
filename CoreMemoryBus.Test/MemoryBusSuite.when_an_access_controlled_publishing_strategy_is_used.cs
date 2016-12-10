using System;
using CoreMemoryBus.Logger;
//using CoreMemoryBus.Logger;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.PublishingStrategies;
using CoreMemoryBus.Util;
using Moq;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    public partial class MemoryBusSuite
    {
        public class TestPublishingStrategy : IPublishingStrategy
        {
            public Action TestAction { get; set; }

            public TestPublishingStrategy(Action testAction)
            {
                TestAction = testAction;
            }

            public void Publish(Message message)
            {
                TestAction();
            }
        }

        [TestFixture]
        public class when_an_access_controlled_publishing_strategy_is_used
        {
            [Test]
            public void a_message_is_executed_if_not_access_controlled()
            {
                bool hasExecuted = false;
                var innerStrategy = new TestPublishingStrategy(() => hasExecuted = true);

                var aclMock = new Mock<IAccessControlList>();
                aclMock.Setup(x => x.IsDenied(It.IsAny<string[]>(), It.IsAny<Type>())).Returns(true);
                aclMock.Setup(x => x.IsGranted(It.IsAny<string[]>(), It.IsAny<Type>())).Returns(false);

                var loggerMock = new Mock<ILogger>();

                var decoratedStrategy = new AccessControlListStrategyDecorator(aclMock.Object, loggerMock.Object, innerStrategy);

                decoratedStrategy.Publish(new TestMessage());

                Assert.IsTrue(hasExecuted);
            }

            public class TestAccessControlledMessage : Message, IAccessControlledMessage
            {
                public TestAccessControlledMessage(string[] principals)
                {
                    Principals = principals;
                }

                public string[] Principals { get; private set; }
            }

            [Test]
            public void a_message_is_not_executed_if_denied()
            {
                bool hasExecuted = false;
                var innerStrategy = new TestPublishingStrategy(() => hasExecuted = true);

                var aclMock = new Mock<IAccessControlList>();
                aclMock.Setup(x => x.IsDenied(It.IsAny<string[]>(), It.IsAny<Type>())).Returns(true);

                var loggerMock = new Mock<ILogger>();

                var decoratedStrategy = new AccessControlListStrategyDecorator(aclMock.Object, loggerMock.Object, innerStrategy);

                decoratedStrategy.Publish(new TestAccessControlledMessage(new[] { "Anyone" }));

                Assert.IsFalse(hasExecuted);
            }

            [Test]
            public void a_message_is_not_executed_if_not_granted()
            {
                bool hasExecuted = false;
                var innerStrategy = new TestPublishingStrategy(() => hasExecuted = true);

                var aclMock = new Mock<IAccessControlList>();
                aclMock.Setup(x => x.IsGranted(It.IsAny<string[]>(), It.IsAny<Type>())).Returns(false);

                var loggerMock = new Mock<ILogger>();

                var decoratedStrategy = new AccessControlListStrategyDecorator(aclMock.Object, loggerMock.Object, innerStrategy);

                decoratedStrategy.Publish(new TestAccessControlledMessage(new[] { "Anyone" }));

                Assert.IsFalse(hasExecuted);
            }
        }
    }
}