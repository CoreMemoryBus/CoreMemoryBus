using System;

namespace CoreMemoryBus.Logger
{
    public class NullLogger : ILogger
    {
        public void Flush(TimeSpan? maxTimeToWait = null)
        {
            
        }

        public void Fatal(string format, params object[] args)
        {
            
        }

        public void Error(string format, params object[] args)
        {
            
        }

        public void Info(string format, params object[] args)
        {
            
        }

        public void Debug(string format, params object[] args)
        {
            
        }

        public void Warn(string format, params object[] args)
        {
            
        }

        public void Trace(string format, params object[] args)
        {
            
        }

        public void FatalException(Exception exc, string format, params object[] args)
        {
            
        }

        public void ErrorException(Exception exc, string format, params object[] args)
        {
            
        }

        public void InfoException(Exception exc, string format, params object[] args)
        {
            
        }

        public void DebugException(Exception exc, string format, params object[] args)
        {
            
        }

        public void TraceException(Exception exc, string format, params object[] args)
        {
            
        }
    }
}