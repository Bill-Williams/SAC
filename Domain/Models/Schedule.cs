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
        [Column(Order = 1)]
        public Guid ClubId { get; set; }

        public virtual Club Club { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(Order = 2)]
        public DateTime Date { get; set; }

        //[ForeignKey("TournamentId")]
        //public virtual Tournament Tournament { get; set; }
    }
}
