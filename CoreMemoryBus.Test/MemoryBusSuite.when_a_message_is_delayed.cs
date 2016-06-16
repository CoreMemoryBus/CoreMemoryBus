using System;
using CoreMemoryBus.Logger;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.PublishingStrategies;
using CoreMemoryBus.Util;
using Moq;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    partial class MemoryBusSuite
    {
        public class FakePublishingStrategy : IPublishingStrategy
        {
            public void Publish(Message message)
            {
                // Simulate some excessively long operation
            }
        }
        
        public class when_a_message_is_delayed
        {
            [Test]
            public void it_can_be_logged_with_a_message_timer()
            {
                var loggerMock = new Mock<ILogger>();
                string output = String.Empty;
                loggerMock
                    .Setup(x => x.Warn(It.IsAny<string>(), It.IsAny<object[]>()))
                    .Callback((string format, object[] args) => output = String.Format(format, args));

                var stopwatchMock = new Mock<IStopwatch>();
                stopwatchMock.SetupGet(x => x.Elapsed).Returns(new TimeSpan(0, 0, 0, 0, 11));
                var stopwatch = stopwatchMock.Object;

                var messageTimer = new Util.MessageTimer(() => stopwatch) { LogThreshold = new TimeSpan(0, 0, 0, 0, 10) };

                var strategy = new FakePublishingStrategy();
                var decoratedStrategy = new MessageTimerStrategyDecorator(messageTimer, loggerMock.Object, strategy);

                decoratedStrategy.Publish(new TestMessage());

                Assert.IsTrue(output.Contains("Slow publication for message"));
            }
        }
    }
}