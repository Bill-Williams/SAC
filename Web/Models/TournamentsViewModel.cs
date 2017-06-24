using SAC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAC.Web.Models
{
    public class TournamentResultViewModel
    {
        public Tournament Tournament { get; set; }

        public IEnumerable<Group> Groups { get; set; }

        public IEnumerable<Class> Classes { get; set; }

        public IEnumerable<RankedCompetitor> Competitors { get; set; }
    }

    public class RankedCompetitor
    {
        public int Rank { get; set; }

        public Competitor Competitor { get; set; }
    }
}