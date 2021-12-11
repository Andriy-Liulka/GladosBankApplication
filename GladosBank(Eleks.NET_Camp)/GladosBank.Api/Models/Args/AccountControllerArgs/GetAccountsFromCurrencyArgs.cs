using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.AccountControllerArgs
{
    public class GetAccountsFromCurrencyArgs
    {
        [Required]
        public string CurrencyCode { get; set; }
        [Required]
        public string Login { get; set; }
    }
}
