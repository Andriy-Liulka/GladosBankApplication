using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain
{
    class Customer : User
    {
        public Account _Account { get; set; }
        public void TransferMoney() { }
    }
}
