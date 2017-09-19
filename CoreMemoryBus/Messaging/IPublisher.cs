using System;
using System.Text;
using CoreMemoryBus.Messages;

namespace CoreMemoryBus.Messaging
{
    public interface IPublisher
    {
        void Publish(Message message);
    }
}
