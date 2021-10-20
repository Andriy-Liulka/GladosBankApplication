using GladosBank.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(Currency))]
        public string CurrencyCode { get; set; }
        public virtual Currency Currency { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }
        [Required]
        public DateTime DateOfCreating { get; set; }

    }
}
