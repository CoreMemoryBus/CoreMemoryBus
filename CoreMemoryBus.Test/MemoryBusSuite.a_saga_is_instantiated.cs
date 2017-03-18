using System;
using System.Linq;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Sagas;
using CoreMemoryBus.Util;
using Moq;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    public partial class MemoryBusSuite
    {
        public static class SagaMessages
        {
            public class CreateSaga : Message, ICorrelatedMessage<Guid>
            {
                public Guid CorrelationId { get; private set; }

                public CreateSaga(Guid correlationId)
                {
                    CorrelationId = correlationId;
                }
            }
            public class CreateSagaWithValue : Message, ICorrelatedMessage<Guid>
            {
                public Guid CorrelationId { get; private set; }
                public int Value { get; private set; }

                public CreateSagaWithValue(Guid correlationId, int value)
                {
                    CorrelationId = correlationId;
                    Value = value;
                }
            }

            public class AddSagaData : Message, ICorrelatedMessage<Guid>
            {
                public Guid CorrelationId { get; private set; }
                public int Value { get; private set; }

                public AddSagaData(Guid correlationId, int value)
                {
                    CorrelationId = correlationId;
                    Value = value;
                }
            }

            public class CompleteSaga : Message, ICorrelatedMessage<Guid>
            {
                public Guid CorrelationId { get; private set; }

                public CompleteSaga(Guid correlationId)
                {
                    CorrelationId = correlationId;
                }
            }
        }

        public class TestSaga :
            Saga,
            IAmTriggeredBy<SagaMessages.CreateSaga>,
            IAmTriggeredBy<SagaMessages.CreateSagaWithValue>,
            IHandle<SagaMessages.AddSagaData>,
            IHandle<SagaMessages.CompleteSaga>,
            ISaga
        {
            public int Value { get; private set; } // Example state variable.

            public int MessageCount { get; private set; } // Example variable that can only be known when the saga is complete.

            public Guid CorrelationId { get; private set; }

            public bool IsComplete { get; private set; }

            public void Handle(SagaMessages.CreateSaga message)
            {
                CorrelationId = message.CorrelationId;
                IncrementMessageCount();
            }

            private void IncrementMessageCount()
            {
                MessageCount += 1;
            }

            public void Handle(SagaMessages.CreateSagaWithValue message)
            {
                CorrelationId = message.CorrelationId;
                Value = message.Value;
                IncrementMessageCount();
            }

            public void Handle(SagaMessages.AddSagaData message)
            {
                Value += message.Value;
                IncrementMessageCount();
            }

            public void Handle(SagaMessages.CompleteSaga message)
            {
                IncrementMessageCount();
                IsComplete = true;
            }
        }

        private static IGuidFactory CreateMockGuidFactory()
        {
            var guidProviderMock = new Mock<IGuidFactory>();
            guidProviderMock.SetupSequence(x => x.NewGuid())
                .Returns(new Guid("00000000-0000-0000-0000-000000000001"))
                .Returns(new Guid("00000000-0000-0000-0000-000000000002"))
                .Returns(new Guid("00000000-0000-0000-0000-000000000003"))
                .Returns(new Guid("00000000-0000-0000-0000-000000000004"))
                .Returns(new Guid("00000000-0000-0000-0000-000000000005"))
                .Returns(new Guid("00000000-0000-0000-0000-000000000006"))
                .Returns(new Guid("00000000-0000-0000-0000-000000000007"))
                .Returns(new Guid("00000000-0000-0000-0000-000000000008"))
                .Returns(new Guid("00000000-0000-0000-0000-000000000009"))
                .Returns(new Guid("00000000-0000-0000-0000-00000000000A"))
                .Returns(new Guid("00000000-0000-0000-0000-00000000000B"))
                .Returns(new Guid("00000000-0000-0000-0000-00000000000C"))
                .Returns(new Guid("00000000-0000-0000-0000-00000000000D"))
                .Returns(new Guid("00000000-0000-0000-0000-00000000000E"))
                .Returns(new Guid("00000000-0000-0000-0000-00000000000F"));
            var guidProvider = guidProviderMock.Object;
            return guidProvider;
        }

        [TestFixture]
        public class a_saga_is_instantiated
        {
            [Test]
            public void by_one_or_more_trigger_messages()
            {
                var guidProvider = CreateMockGuidFactory();

                var theMessageBus = new MemoryBus();
                // A SagaContainer is created to manage the lifetime of Sagas of a given type.
                var sagas = new SagaContainer<TestSaga>();
                Assert.That<int>(sagas.Count, Is.Zero);

                // The container subscribes to the Message type which it forwards to the saga instances.
                theMessageBus.Subscribe(sagas);

                // Create the saga with the default trigger.
                var createSaga = new SagaMessages.CreateSaga(guidProvider.NewGuid());
                var correlationId1 = createSaga.CorrelationId;
                theMessageBus.Publish(createSaga);
                Assert.That<int>(sagas.Count, Is.EqualTo(1));

                // When created, the saga adopts the correlationId of the trigger message that created it.
                var newSaga1 = (TestSaga)sagas.FirstOrDefault(x => x.CorrelationId == correlationId1);
                Assert.That(newSaga1 != null, Is.True);
                Assert.That(newSaga1.Value, Is.EqualTo(0));

                //Create the saga with an alternate trigger
                var createSagaWithValue = new SagaMessages.CreateSagaWithValue(guidProvider.NewGuid(), 10);
                var correlationId2 = createSagaWithValue.CorrelationId;
                theMessageBus.Publish(createSagaWithValue);
                Assert.That<int>(sagas.Count, Is.EqualTo(2));

                var newSaga2 = (TestSaga)sagas.FirstOrDefault(x => x.CorrelationId == correlationId2);
                Assert.That(newSaga2 != null, Is.True);
                Assert.That(newSaga2.Value, Is.EqualTo(10));
            }
        }

        [TestFixture]
        public class when_a_message_is_published_to_a_saga
        {
            [Test]
            public void it_is_ignored_if_there_is_no_correlated_saga()
            {
                var guidProvider = CreateMockGuidFactory();

                var theMessageBus = new MemoryBus();
                var sagas = new SagaContainer<TestSaga>();
                theMessageBus.Subscribe(sagas);

                var createSaga = new SagaMessages.CreateSaga(guidProvider.NewGuid());
                var correlationId1 = createSaga.CorrelationId;
                theMessageBus.Publish(createSaga);
                var newSaga = (TestSaga)sagas.FirstOrDefault(x => x.CorrelationId == correlationId1);

                var addData = new SagaMessages.AddSagaData(guidProvider.NewGuid(), 11);
                theMessageBus.Publish(addData);
                Assert.That(newSaga.Value, Is.EqualTo(0));
            }
        }

        [TestFixture]
        public class when_a_saga_is_completed
        {
            [Test]
            public void it_can_respond_to_queries_of_its_state()
            {
                var guidProvider = CreateMockGuidFactory();

                var theMessageBus = new MemoryBus();
                var sagas = new SagaContainer<TestSaga>();
                theMessageBus.Subscribe(sagas);

                var createSaga = new SagaMessages.CreateSaga(guidProvider.NewGuid());
                var correlationId1 = createSaga.CorrelationId;
                theMessageBus.Publish(createSaga);

                bool isComplete = false;
                var reply = new ActionReplyEnvelope<Messages.SagaMessages.SagaCompleteReply>(x => { isComplete = x.IsComplete; });

                theMessageBus.Publish(new Messages.SagaMessages.QuerySagaComplete(correlationId1, reply));

                Assert.That(isComplete, Is.False);

                theMessageBus.Publish(new SagaMessages.CompleteSaga(correlationId1));
                theMessageBus.Publish(new Messages.SagaMessages.QuerySagaComplete(correlationId1, reply));

                Assert.That(isComplete, Is.True);
            }

            [Test]
            public void it_can_be_deleted()
            {
                var guidProvider = CreateMockGuidFactory();

                var theMessageBus = new MemoryBus();
                var sagas = new SagaContainer<TestSaga>();
                theMessageBus.Subscribe(sagas);

                var createSaga = new SagaMessages.CreateSaga(guidProvider.NewGuid());
                var correlationId1 = createSaga.CorrelationId;
                theMessageBus.Publish(createSaga);
                theMessageBus.Publish(new SagaMessages.CompleteSaga(correlationId1));

                theMessageBus.Publish(new Messages.SagaMessages.DeleteSaga(correlationId1));

                var completeSaga = (TestSaga)sagas.FirstOrDefault(x => x.CorrelationId == correlationId1);
                Assert.That(completeSaga, Is.Null);
            }
        }
    }
}