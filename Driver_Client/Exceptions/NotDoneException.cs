using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver_Client
{
    internal class NotDoneException : Exception
    {
        public NotDoneException(string message) : base(message)
        {

        }
    }
}
