using System;

namespace Driver_Client
{
    internal class BlockedException : Exception
    {
        public int profile;
        public BlockedException(int profile)
        {
            this.profile = profile;
        }

    }
}