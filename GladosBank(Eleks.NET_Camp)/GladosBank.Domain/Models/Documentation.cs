using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladosBank.Domain
{
    class Documentation
    {
        [Key]
        public string Id { get; set; }
        public string DescriptionDocumentation { get; set; }
    }
}
