using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GladosBank.Api.Models.Args.UserControllerArgs
{
    public class PaginatedArgs
    {
        [Required]
        public int pageIndex { get; set; }
        [Required]
        public int pageSize { get; set; }
    }
}
