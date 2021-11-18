using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class DonotHaveRoleException : BusinessLogicException
    {
        public DonotHaveRoleException(string login) : base($"{login} doesn't have role !") { }
    }
}
