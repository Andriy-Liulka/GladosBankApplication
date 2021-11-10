using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.UserControllerArgs
{
    public sealed class LoginUserArgs
    {
        [MaxLength(200)]
        [MinLength(1)]
        [Required]
        public string Login { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }
}
