using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class InvalidUserLoginException : Exception
    {
        public InvalidUserLoginException(string Login) : base($"You entered Login-> \"{Login}\" that doesn't exist of in database !") { }
    }
}
