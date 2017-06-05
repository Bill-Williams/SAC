﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.Models
{
    public class Class : BaseEntity
    {
        [Required]
        [MaxLength(2)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public bool Known { get; set; }

        [Required]
        public int MaximumYardage { get; set; }

        [MaxLength(100)]
        public string Restrictions { get; set; }

        public Guid ColorId { get; set; }

        public virtual Color Color { get; set; }
    }
}
