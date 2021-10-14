using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain
{
    class Account
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        private readonly DateTime DateOfCreating;

        public Account()
        {
            DateOfCreating = DateTime.Now;
        }
    }
}
