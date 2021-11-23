using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class NotYourAccountException : BusinessLogicException
    {
        public NotYourAccountException(int id) : base($"Account with id {id} doesn't your!") { }
    }
}
