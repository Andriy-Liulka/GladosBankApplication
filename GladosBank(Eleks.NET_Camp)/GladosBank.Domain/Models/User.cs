using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain
{
    class User
    {
        public int Id { get; set; }
        protected long Phone { get; set; }
        protected string Email { get; set; }
        protected string Login { get; set; }
        protected string Password { get; set; }
        protected Information _Information { get; set; }
    }
}
