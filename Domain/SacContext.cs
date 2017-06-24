using System.Data.Entity;
using SAC.Domain.Models;

namespace SAC.Domain
{
    public class SacContext : DbContext
    {
        private static SacContext _current { get; set; }

        static SacContext()
        {
            Database.SetInitializer<SacContext>(null);
        }

        public SacContext()
            : base("SacConn")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public SacContext(string connectionStringName)
            : base(connectionStringName)
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Color> Colors { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<AspNetRole> Roles { get; set; }
        public DbSet<AspNetUser> Users { get; set; }
        public DbSet<AspNetUserClaim> UserClaims { get; set; }
        public DbSet<AspNetUserLogin> UserLogins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .HasMany(x => x.AspNetRoles)
                .WithMany(x => x.AspNetUsers)
            .Map(x =>
            {
                x.ToTable("AspNetUserRoles");
                x.MapLeftKey("UserId");
                x.MapRightKey("RoleId");
            });

            modelBuilder.Entity<AspNetUser>()
                .HasMany(x => x.Clubs)
                .WithMany(x => x.Users)
            .Map(x =>
            {
                x.ToTable("AspNetUserClubs");
                x.MapLeftKey("UserId");
                x.MapRightKey("ClubId");
            });
        }
    }
}
