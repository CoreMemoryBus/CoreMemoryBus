using System;

namespace CoreMemoryBus.Util
{
    public interface IMessageTimer : IStopwatch
    {
        TimeSpan LogThreshold { get; set; }
    }
}
