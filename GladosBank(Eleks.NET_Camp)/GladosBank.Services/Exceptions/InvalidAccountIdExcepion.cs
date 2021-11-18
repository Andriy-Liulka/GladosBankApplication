using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class InvalidAccountIdExcepion : BusinessLogicException
    {
        public InvalidAccountIdExcepion(int id) : base($"Account with id-> {id} doesn't exist of !"){}
    }
}
