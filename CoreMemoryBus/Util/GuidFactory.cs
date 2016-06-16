using System;

namespace CoreMemoryBus.Util
{
    public class GuidFactory : IGuidFactory
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}