using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain.Models_DTO
{
    class AccountDTO
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }
}
