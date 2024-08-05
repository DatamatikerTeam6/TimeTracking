using DogRallyAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DogRallyAPI.Data
{
    public class TimeTrackingDbContext : IdentityDbContext<ApplicationUser>
    {
        // Database context
        public TimeTrackingDbContext(DbContextOptions<TimeTrackingDbContext> options) : base(options)
        {

        }

        // Database tables
        public DbSet<Timetracking> Timetrackings { get; set; } = default!;


        // Database tablenames override
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure Identity tables are created
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Timetracking>().ToTable("Timetracking");
          
        }
    }
}
