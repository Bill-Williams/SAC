using SAC.Domain;
using SAC.Domain.Models;
using System;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Collections.Generic;

namespace SAC.Web.Extensions
{
    public static class IdentityExtensions
    {
        public static AspNetUser GetAspNetUser(this IIdentity identity, SacContext context)
        {
            return context.Users.Find(Guid.Parse(identity.GetUserId()));
        }

        public static IEnumerable<Club> GetClubs(this IIdentity identity, SacContext context)
        {
            var id = Guid.Parse(identity.GetUserId());
            return context.Users.Include("Clubs").First(u => u.Id == id).Clubs;
        }
    }
}
