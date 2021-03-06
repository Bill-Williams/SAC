﻿using System;
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
            Schedules = new HashSet<Schedule>();
        }

        public bool Completed { get; set; }

        public virtual ICollection<Competitor> Competitors { get; set; }
        
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
