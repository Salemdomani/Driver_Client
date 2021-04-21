using System;

namespace Driver_Client
{
    internal class AlreadyLikedException : Exception
    {
        public AlreadyLikedException(string message) : base(message)
        {
        }
    }
}