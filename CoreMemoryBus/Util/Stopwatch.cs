using System;

namespace CoreMemoryBus.Util
{
    public class Stopwatch : IStopwatch
    {
        private readonly System.Diagnostics.Stopwatch _impl;

        public Stopwatch()
        {
            _impl = new System.Diagnostics.Stopwatch();
        }

        public void Start() { _impl.Start(); }
        public void Restart() { _impl.Restart(); }
        public void Stop() { _impl.Stop(); }

        public TimeSpan Elapsed => _impl.Elapsed;
    }
}
