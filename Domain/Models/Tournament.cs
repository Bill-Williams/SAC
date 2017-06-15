using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAC.Domain.Models
{
    public class Tournament : BaseEntity
    {
        public Tournament()
        {
            Competitors = new HashSet<Competitor>();
        }

        public bool Completed { get; set; }

        public ICollection<Competitor> Competitors { get; set; }

        [Required]
        public Guid ScheduleId { get; set; }

        public virtual Schedule Schedule { get; set; }
    }
}
