using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.Models
{
    public class Competitor : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Archer { get; set; }

        public int? Score { get; set; }

        public int? Bonus { get; set; }

        [Required]
        public Guid TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        [Required]
        public Guid ClassId { get; set; }

        public virtual Class Class { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
