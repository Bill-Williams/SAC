using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SAC.Domain.Models
{
    public partial class AspNetUserClaim : BaseEntity
    {
        [Required]
        [Column(Order = 1)]
        public Guid UserId { get; set; }

        [Column(Order = 2)]
        public string ClaimType { get; set; }

        [Column(Order = 3)]
        public string ClaimValue { get; set; }

        [ForeignKey("UserId")]
        public virtual AspNetUser AspNetUser { get; }
    }
}
