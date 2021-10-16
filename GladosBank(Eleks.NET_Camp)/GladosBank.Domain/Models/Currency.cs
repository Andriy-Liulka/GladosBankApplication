using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain.Models
{
    class Currency
    {
        [Key]
        public string Id { get; set; }
        public string Symbol { get; set; }
    }
}
