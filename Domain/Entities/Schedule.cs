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
        public Guid ClubId { get; set; }
        [ForeignKey("ClubId")]
        [Column(Order = 1)]
        public virtual Club Club { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
