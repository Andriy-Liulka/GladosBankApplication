using GladosBank.Domain.Models_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.UserControllerArgs
{
    public sealed class CreateUserArgs
    {
        public UserDTO MyUser { get; set; }
        public string Role { get; set; }
    }
}
