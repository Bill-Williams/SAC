using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SAC.Domain;

namespace SAC.Web.Models
{
    public class SacDbContext : SacContext
    {
        public SacDbContext()
            : base("SacConn")
        {
        }

        public static SacDbContext Create()
        {
            return new SacDbContext();
        }
    }
}