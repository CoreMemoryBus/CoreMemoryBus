using System;
using CoreMemoryBus.Messages;
using CoreMemoryBus.Messaging;
using CoreMemoryBus.Util;

namespace CoreMemoryBus.PublishingStrategies
{
    public class ExceptionProcessingStrategyDecorator : IPublishingStrategy
    {
        private readonly IPublishingStrategy _implementation;
        private readonly IGuidFactory _guidFactory;

        public ExceptionProcessingStrategyDecorator(IGuidFactory guidFactory, IPublishingStrategy implementation)
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