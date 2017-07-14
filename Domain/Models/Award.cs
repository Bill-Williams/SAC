using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.Models
{
    public class Award : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Icon { get; set; }
    }
}
