using System;

namespace CoreMemoryBus.Util
{
    public interface IMessageTimer
    {
        TimeSpan LogThreshold { get; set; }

        IStopwatch CreateStopwatch();
    }
}