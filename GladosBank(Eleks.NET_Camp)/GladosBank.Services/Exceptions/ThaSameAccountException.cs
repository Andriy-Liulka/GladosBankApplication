using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class ThaSameAccountException : BusinessLogicException
    {
        public ThaSameAccountException() : base($"SourceAccount and DistinationAccount are the same !") { }
    }
}
