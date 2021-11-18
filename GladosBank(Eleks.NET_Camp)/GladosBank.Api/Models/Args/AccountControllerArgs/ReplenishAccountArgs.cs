using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.AccountControllerArgs
{
    public class ReplenishAccountArgs
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
