using System;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.PublishingStrategies;
using CoreMemoryBus.Util;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

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

        public class ListMessageSink : IMessageSink
        {
            public readonly List<Message> SlowMessages = new List<Message>();

            public void ReceiveMessage(Message msg)
            {
                SlowMessages.Add(msg);
            }
        }
        
        public class when_a_message_is_delayed
        {
            [Test]
            public void it_can_be_logged_with_a_message_timer()
            {
                var stopwatchMock = new Mock<IStopwatch>();
                stopwatchMock.SetupGet(x => x.Elapsed).Returns(new TimeSpan(0, 0, 0, 0, 11));
                var stopwatch = stopwatchMock.Object;

                var messageTimer = new Util.MessageTimer(stopwatch) { LogThreshold = new TimeSpan(0, 0, 0, 0, 10) };

                var slowMessageSink = new ListMessageSink();
                var strategy = new FakePublishingStrategy();
                var decoratedStrategy = new MessageTimerPublishingStrategy(strategy, messageTimer, slowMessageSink);

                decoratedStrategy.Publish(new TestMessage());

                Assert.That(slowMessageSink.SlowMessages.Count, Is.EqualTo(1));
                Assert.That(slowMessageSink.SlowMessages[0].GetType(), Is.EqualTo(typeof(MessageTimerMessages.SlowPublication)));
            }
        }
    }
}