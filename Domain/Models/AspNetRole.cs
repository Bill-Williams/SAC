using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SAC.Domain.Models
{
    public partial class AspNetRole : BaseEntity
    {
        public AspNetRole()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
