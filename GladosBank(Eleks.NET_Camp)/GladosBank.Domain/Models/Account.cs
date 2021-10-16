using GladosBank.Domain.Models;
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
        public Customer _Customer_Id { get; set; }
        public Currency _Currency { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        private readonly DateTime DateOfCreating;

        public Account()
        {
            DateOfCreating = DateTime.Now;
        }
    }
}
