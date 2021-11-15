using GladosBank.Api.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.AccountControllerArgs
{
    public sealed class CreateAccountArgs
    {
        [Required]
        public AccountDto Account { get; set; }
        [Required]
        public int CustomerId { get; set; }
    }
}
