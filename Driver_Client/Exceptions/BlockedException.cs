using System;

namespace Driver_Client
{
    internal class BlockedException : Exception
    {
        public int profile;

        public BlockedException(string message) : base(message)
        {
            profile = Convert.ToInt32(message.Split()[3]);
        }

    }
}