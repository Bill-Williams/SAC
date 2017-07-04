using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SAC.Domain.Models;

namespace SAC.Web.Models
{
    public class CompetitorListItemViewModel
    {
        public string Archer { get; set; }

        public int EntryOrder { get; set; }
    }
}