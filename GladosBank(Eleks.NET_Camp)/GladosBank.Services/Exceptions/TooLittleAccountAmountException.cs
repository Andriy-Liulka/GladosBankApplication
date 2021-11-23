using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class TooLittleAccountAmountException : BusinessLogicException
    {
        public TooLittleAccountAmountException() : base("Amount on source account is too little !")
        {
        }
    }
}
