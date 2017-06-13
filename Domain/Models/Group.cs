using SAC.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.Models
{
    public class Group : BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public int SortOrder { get; set; }
    }
}
