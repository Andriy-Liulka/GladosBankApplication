using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class AddingExistUserException : BusinessLogicException
    {
        public AddingExistUserException() : base("You try to add user that already exist of !") { }
    }
}
