using System;
using System.Diagnostics;

namespace CoreMemoryBus.Util
{
    public static class Ensure
    {
        //[Conditional("DEBUG")]
        public static void ArgumentIsNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
