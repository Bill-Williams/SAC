﻿using System;
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
        public virtual Archer Archer { get; set; }

        public virtual Class Class { get; set; }

        public int Score { get; set; }

        public int Bonus { get; set; }
    }
}