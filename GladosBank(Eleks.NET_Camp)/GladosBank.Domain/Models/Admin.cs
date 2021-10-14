using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain
{
    class Admin
    {
        public int Id { get; set; }
        public void StopServer() { }
        public void DeleteUser() { }
        public void ManageServer() { }
        public static Documentation GetDocumentation() => null;
    }
}
