using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.DTO
{
    public sealed class AccountDto
    {
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }
}
