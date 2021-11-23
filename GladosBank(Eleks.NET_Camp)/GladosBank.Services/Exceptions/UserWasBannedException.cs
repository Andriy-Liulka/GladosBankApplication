using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class UserWasBannedException : BusinessLogicException
    {
        public UserWasBannedException(string login) : base($"User with login {login} was banned !") { }
    }
}
