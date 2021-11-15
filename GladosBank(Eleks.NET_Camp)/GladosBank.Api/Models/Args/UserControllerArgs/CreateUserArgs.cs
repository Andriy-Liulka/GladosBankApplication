using GladosBank.Domain.Models_DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.UserControllerArgs
{
    public sealed class CreateUserArgs
    {
        [Required]
        public UserDto MyUser { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
