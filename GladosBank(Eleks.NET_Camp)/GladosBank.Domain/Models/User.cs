using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Phone { get; set; }
        [Required]
        [MinLength(5)]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Login { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public  bool IsActive { get; set; }
    }
}
