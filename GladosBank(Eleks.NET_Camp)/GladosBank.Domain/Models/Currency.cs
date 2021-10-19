using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain.Models
{
    public class Currency
    {
        [Key]
        public string Code { get; set; }
        public string Symbol { get; set; }
    }
}
