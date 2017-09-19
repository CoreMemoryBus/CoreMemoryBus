using System;

namespace CoreMemoryBus.Util
{
    public class MessageTimer : IMessageTimer
    {
        private readonly IStopwatch _stopwatch;

        public MessageTimer(IStopwatch stopwatch)
        {
            _stopwatch = stopwatch;
        }

        public TimeSpan LogThreshold { get; set; }

        public TimeSpan Elapsed => _stopwatch.Elapsed;

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Restart()
        {
            _stopwatch.Restart();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }
    }
}
