using System;
using System.Text;

namespace CoreMemoryBus.Handlers
{
    /// <summary>
    /// This interface represents an implementation of a function that handles delivery of a specific message type.
    /// An implementing class subscribes to publications of a message type, typically with a MemoryBus or ProxyPublisher derived
    /// class. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHandle<in T>
    {
        void Handle(T message);
    }
}
