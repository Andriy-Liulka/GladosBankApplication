using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain
{
    public class OperationsHistory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateTimet{get; set; }
        [Required]
        public string Description { get; set; }
    }
}
