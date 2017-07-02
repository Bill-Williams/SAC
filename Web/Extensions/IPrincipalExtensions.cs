using SAC.Domain;
using SAC.Domain.Models;
using System;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Collections.Generic;

namespace SAC.Web.Extensions
{
    public static class IPrincipalExtensions
    {
        public static AspNetUser GetAspNetUser(this IPrincipal principal, SacContext context)
        {
            return context.Users.Find(Guid.Parse(principal.Identity.GetUserId()));
        }

        public static IQueryable<Club> GetClubs(this IPrincipal principal, SacContext context)
        {
            if(principal.IsInRole("Tech Admin"))
            {
                return context.Clubs.AsQueryable(); // .Include("Contacts");
            }
            else
            {
                var id = Guid.Parse(principal.Identity.GetUserId());
                return context.Clubs.Include("Users").Where(c => c.Users.Any(u => u.Id == id));
            }
        }

        public static IQueryable<Tournament> GetTournaments(this IPrincipal principal, SacContext context)
        {
            if (principal.IsInRole("Tech Admin"))
            {
                return context.Tournaments.Include("Schedules.Club"); //.Include("Competitors.Class.Group");
            }
            else
            {
                var clubIds = principal.GetClubs(context).Select(c => c.Id);
                return context.Tournaments.Include("Schedules.Club").Where(t => t.Schedules.Select(s => s.Club.Id).Intersect(clubIds).Count() > 0);
            }
        }

        public static IQueryable<Schedule> GetSchedules(this IPrincipal principal, SacContext context)
        {
            if (principal.IsInRole("Tech Admin"))
            {
                return context.Schedules.Include("Club");
            }
            else
            {
                var clubIds = principal.GetClubs(context).Select(c => c.Id);
                return context.Schedules.Include("Club").Where(s => clubIds.Contains(s.Club.Id));
            }
        }
    }
}
