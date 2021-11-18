using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class InvalidUserIdException : BusinessLogicException
    {
        public InvalidUserIdException(int UserId) : base($"You entered userId-> \"{UserId}\" that doesn't exist of in database !") {}
    }
}
