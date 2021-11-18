using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class InvalidRoleException : BusinessLogicException
    {
        public InvalidRoleException(string role) : base($"Role of {role} doesn't exist of !!") { }
    }
}
