using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.AccountControllerArgs
{
    public class KeepHistoryOfOperationArgs
    {
        [Required]
        public string Description { get; set; }
    }
}
