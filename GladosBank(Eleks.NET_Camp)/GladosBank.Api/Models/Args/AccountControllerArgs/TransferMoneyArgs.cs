using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.AccountControllerArgs
{
    public class TransferMoneyArgs
    {
        [Required ]
        public decimal Amount { get; set; }
        [Required]
        public int sourceId { get; set; }
        [Required]
        public int destinationId { get; set; }

    }
}
