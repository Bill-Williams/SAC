using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.Models
{
    public class Schedule : BaseEntity
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "From Date")]
        [Column(Order = 1)]
        public DateTime FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "To Date")]
        [Column(Order = 2)]
        public DateTime ToDate { get; set; }

        [Column(Order = 3)]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        [Column(Order = 4)]
        public Guid ClubId { get; set; }
        public virtual Club Club { get; set; }

        [Column(Order = 5)]
        public Guid? TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        [Display(Name = "Dates")]
        public virtual string DisplayShortDate
        {
            get
            {
                if(this.FromDate == this.ToDate)
                {
                    return this.FromDate.ToShortDateString();
                }
                return $"{this.FromDate.ToShortDateString()} - {this.ToDate.ToShortDateString()}";
            }
        }

        public virtual string DisplayClubWithShortDate
        {
            get
            {
                if (this.FromDate == this.ToDate)
                {
                    return $"{this.Club.ShortName} ({this.FromDate.ToShortDateString()})";
                }
                return $"{this.Club.ShortName} ({this.FromDate.ToShortDateString()} - {this.ToDate.ToShortDateString()})";
            }
        }

        [Display(Name = "Description")]
        public virtual string BasicDescription
        {
            get
            {
                return this.Description ?? "Standard SAC Tournament";
            }
        }
    }
}
