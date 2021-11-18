using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public abstract class BusinessLogicException : Exception
    {
        protected BusinessLogicException(string message):base(message){}
    }
}
