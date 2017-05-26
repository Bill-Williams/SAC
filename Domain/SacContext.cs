using System.Data.Entity;

namespace Domain
{
    public class SacContext : DbContext
    {

        static SacContext()
        {
            Database.SetInitializer<SacContext>(null);
        }

        public SacContext()
            : base("SAC") { }

        public SacContext(string connectionStringName)
            : base(connectionStringName) { }

        public DbSet<Color> Colors { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Archer> Archers { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) { }
    }
}
