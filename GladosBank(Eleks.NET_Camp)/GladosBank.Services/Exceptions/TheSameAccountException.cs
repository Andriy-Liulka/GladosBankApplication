using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class TheSameAccountException : BusinessLogicException
    {
        public TheSameAccountException() : base($"SourceAccount and DistinationAccount are the same !") { }
    }
}
