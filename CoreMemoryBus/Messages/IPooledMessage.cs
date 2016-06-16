namespace CoreMemoryBus.Messages
{
    public interface IPooledMessage
    {
        /// <summary>
        /// Flag to the containing object pool to resume ownership of the 
        /// message.
        /// </summary>
        void Release();
    }
}