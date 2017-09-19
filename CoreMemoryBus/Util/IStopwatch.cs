using System;

namespace CoreMemoryBus.Util
{
    public interface IStopwatch
    {
        void Start();
        void Restart();
        void Stop();
        TimeSpan Elapsed { get; }
    }
}
