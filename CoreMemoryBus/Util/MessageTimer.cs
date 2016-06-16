using System;

namespace CoreMemoryBus.Util
{
    public class MessageTimer : IMessageTimer
    {
        public static readonly TimeSpan Default = new TimeSpan(0, 0, 0, 0, 50);
        private readonly Func<IStopwatch> _stopwatchFactory;

        public MessageTimer(Func<IStopwatch> stopwatchFactory = null)
        {
            Func<IStopwatch> defaultFactory = () => new Stopwatch();
            _stopwatchFactory = stopwatchFactory ?? defaultFactory;
            LogThreshold = Default;
        }

        public TimeSpan LogThreshold { get; set; }
        public IStopwatch CreateStopwatch()
        {
            return _stopwatchFactory();
        }
    }
}