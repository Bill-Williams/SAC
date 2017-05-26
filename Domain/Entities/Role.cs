using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
