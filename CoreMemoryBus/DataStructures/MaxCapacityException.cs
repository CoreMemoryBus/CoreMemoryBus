using System;

namespace CoreMemoryBus.DataStructures
{
    /// <summary>
    /// Thrown when an object pool has loaned out all objects and another is requested
    /// </summary>
    public class MaxCapacityException : Exception
    {
        public int MaxCapacity { get; set; }

        public MaxCapacityException(int maxCapacity)
        {
            MaxCapacity = maxCapacity;
        }
    }
}
