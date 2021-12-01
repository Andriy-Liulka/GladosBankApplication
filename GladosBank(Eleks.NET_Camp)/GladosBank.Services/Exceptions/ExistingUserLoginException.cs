using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class ExistingUserLoginException : BusinessLogicException
    {
        public ExistingUserLoginException(string login) : base($"User with login {login} already exist of") { }
    }
}
