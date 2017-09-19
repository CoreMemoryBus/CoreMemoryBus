using System;
using CoreMemoryBus.Messages;
using NUnit.Framework.Internal;
using CoreMemoryBus.Handlers;

namespace CoreMemoryBus.Test
{
    public partial class MemoryBusSuite : TestSuite
    {
        public MemoryBusSuite()
            : base("MemoryBusSuite")
        { }

        public class TestMessage : Message {
            public bool IsHandledOne { get; set; }
            public bool IsHandledTwo { get; set; }

            public int HandlingCount { get; set; }
        }

        public class TestMessageHandler<TMessage> : IHandle<TMessage> where TMessage : Message
        {
            private Action<TMessage> Action { get; set; }

            public TestMessageHandler(Action<TMessage> action)
            {
                Action = action;
            }

            public void Handle(TMessage message)
            {
                Action(message);
            }
        }
    }
}
