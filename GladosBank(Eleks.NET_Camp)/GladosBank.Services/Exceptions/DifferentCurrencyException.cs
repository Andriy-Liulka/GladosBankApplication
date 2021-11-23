using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Services.Exceptions
{
    public class DifferentCurrencyException : BusinessLogicException
    {
        public DifferentCurrencyException(string sourceCurrrencyCode,string detinationCurrrencyCode) : base($"Source currency -> {sourceCurrrencyCode} and destination currency ->{detinationCurrrencyCode}")
        {
        }
    }
}
