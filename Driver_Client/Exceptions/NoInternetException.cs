using System;

namespace Driver_Client
{
    internal class NoInternetException : Exception
    {
        public NoInternetException(string message) : base(message)
        {

        }
    }
}