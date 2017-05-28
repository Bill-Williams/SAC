using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SAC.Domain;

namespace SAC.Web.Models
{
    public class MigrationDbContext : SacContext
    {
        public MigrationDbContext()
            : base("MigrationConn")
        {
        }

        public static MigrationDbContext Create()
        {
            return new MigrationDbContext();
        }
    }
}