using System;

namespace Driver_Client
{
    internal class AlreadyLikedException : Exception
    {
        public int profile;
        public AlreadyLikedException(string message):base(message)
        {
            profile = Convert.ToInt32(message.Split()[3]);
        }
    }
}