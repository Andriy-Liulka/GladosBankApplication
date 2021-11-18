using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class InvalidCurrencyException : BusinessLogicException
    {
        public InvalidCurrencyException(string Currency) : base($"Customer with currency->{Currency} doesn't not exist of!!") { }
    }
}
