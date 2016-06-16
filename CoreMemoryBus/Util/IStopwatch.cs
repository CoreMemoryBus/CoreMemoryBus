using System;

namespace CoreMemoryBus.Util
{
    public interface IStopwatch
    {
        void Start();
        void Stop();
        TimeSpan Elapsed { get; }
    }
}