using System;
using CoreMemoryBus.Messaging;
using NUnit.Framework;

namespace CoreMemoryBus.Test
{
    public partial class MemoryBusSuite
    {
        [TestFixture]
        public class when_a_messageBus_is_instantiated
        {
            [Test]
            public void by_default_its_name_is_Empty()
            {
                var theMessageBus = new MemoryBus();
                Assert.That(theMessageBus.Name, Is.EqualTo(String.Empty));
            }

            [Test]
            public void by_default_its_id_is_Empty()
            {
                var theMessageBus = new MemoryBus();
                Assert.That(theMessageBus.Id, Is.EqualTo(Guid.Empty));
            }
        }
    }
}