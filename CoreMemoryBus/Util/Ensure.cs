using System;

namespace CoreMemoryBus.Util
{
    public static class Ensure
    {
        public static void ArgumentIsNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}