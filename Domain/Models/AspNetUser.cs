using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SAC.Domain.Models
{
    public partial class AspNetUser : BaseEntity
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetRoles = new HashSet<AspNetRole>();
        }

        [StringLength(256)]
        public string Email { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Two Factor Enabled")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "Lockout End Date")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Access Failed Count")]
        public int AccessFailedCount { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }

        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }

        public virtual ICollection<Club> Clubs { get; set; }

    }
}
