using Microsoft.EntityFrameworkCore;

namespace OpenHackathonWeb.Data
{
    public class HackathonDbContext : DbContext
    {
        public HackathonDbContext(DbContextOptions<HackathonDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Hackathons> Hackathons { get; set; }

        public DbSet<HackathonRegistrations> HackathonRegistrations { get; set; }
    }
}
