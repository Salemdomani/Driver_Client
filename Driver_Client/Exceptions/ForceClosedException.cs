using System;

namespace Driver_Client
{
    internal class ForceClosedException : Exception
    {
        public ForceClosedException(string message) : base(message)
        {
        }

    }
}