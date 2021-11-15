using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class InvalidCustomerException: Exception
    {
        public InvalidCustomerException(int id) : base($"Customer with id->{id} doesn't not exist of!!") { }
    }
}
