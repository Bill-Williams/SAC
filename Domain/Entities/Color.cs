using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Color : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(15)]
        public string HexCode { get; set; }

    }
}
