using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Competitor : BaseEntity
    {
        [Required]
        public int ArcherId { get; set; }
        [ForeignKey("ArcherId")]
        [Column(Order = 1)]
        public virtual Archer Archer { get; set; }

        [Required]
        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        [Column(Order = 1)]
        public virtual Class Class { get; set; }

        public int Score { get; set; }
        public int Bonus { get; set; }
    }
}
