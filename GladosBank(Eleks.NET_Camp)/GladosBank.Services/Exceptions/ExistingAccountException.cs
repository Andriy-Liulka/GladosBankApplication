using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class ExistingAccountException : Exception
    {
        public ExistingAccountException(int id) :base($"Account with Id-> {id} already exist of !") {}
    }
}
