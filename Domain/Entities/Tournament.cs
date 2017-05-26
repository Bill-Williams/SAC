using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Tournament : BaseEntity
    {
        [Required]
        public int ScheduleId { get; set; }
        [ForeignKey("ScheduleId")]
        [Column(Order = 1)]
        public virtual Schedule Schedule { get; set; }

        public ICollection<Competitor> Competitors { get; set; }

    }
}
