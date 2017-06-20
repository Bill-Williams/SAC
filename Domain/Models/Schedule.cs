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
        [Column(Order = 1)]
        public DateTime Date { get; set; }

        [Column(Order = 2)]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        [Column(Order = 3)]
        public Guid ClubId { get; set; }
        public virtual Club Club { get; set; }

        [Column(Order = 4)]
        public Guid? TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual string ShortDate
        {
            get
            {
                return this.Date.ToShortDateString();
            }
        }

        public virtual string BasicDescription
        {
            get
            {
                return this.Description ?? "Standard SAC Tournament";
            }
        }
    }
}
