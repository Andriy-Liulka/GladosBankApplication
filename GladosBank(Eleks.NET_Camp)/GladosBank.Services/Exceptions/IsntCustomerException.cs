using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class IsntCustomerException : Exception
    {
        public IsntCustomerException(string login) : base($"{login} - isn't a customer") { }
    }
}
