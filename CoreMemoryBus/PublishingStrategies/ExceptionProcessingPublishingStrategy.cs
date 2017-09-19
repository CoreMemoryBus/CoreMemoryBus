using System;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.PublishingStrategies
{
    /// <summary>
    /// ExceptionProcessingPublishingStrategy is a decorator class that wraps around a 
    /// concrete publishing strategy (e.g. FlatPublishingStrategy,HeirarchicalPublishingStrategy). 
    /// It provides a last opportunity to handle exceptions thrown when a message is handled.
    /// A subscribing message handler must implement a synchonous handler for the ProcessException message and 
    /// mark the message as handled, otherwise the exception will be rethrown.
    /// </summary>
    public class ExceptionProcessingPublishingStrategy : IPublishingStrategy
    {
        private readonly IPublishingStrategy _implementation;
        private readonly IGuidFactory _guidFactory;

        public ExceptionProcessingPublishingStrategy(IPublishingStrategy implementation, IGuidFactory guidFactory)
        {
            _implementation = implementation;
            _guidFactory = guidFactory;
        }

        public void Publish(Message message)
        {
            try
            {
                _implementation.Publish(message);
            }
            catch (Exception e)
            {
                var cmd = new ProcessException(_guidFactory.NewGuid(), e, message);

                _implementation.Publish(cmd);

                if (!cmd.IsHandled)
                {
                    throw;
                }
            }
        }
    }
}
