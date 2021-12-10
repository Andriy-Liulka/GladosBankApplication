using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    class SmallAmountException : BusinessLogicException
    {
        public SmallAmountException(string amount):base($"You cannot do any operations with \"{amount}\" amount") { }
    }
}
