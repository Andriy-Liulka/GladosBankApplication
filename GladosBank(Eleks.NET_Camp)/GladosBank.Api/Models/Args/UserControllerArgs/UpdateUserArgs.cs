using GladosBank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.UserControllerArgs
{
    public class UpdateUserArgs
    {
        public int Id { get; set; }
        public User NewUser { get; set; }
    }
}
