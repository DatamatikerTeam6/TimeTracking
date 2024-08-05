using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeTrackingServerAPI.Models;

namespace TimeTrackingServerAPI.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
         base(options)
        { }

        public DbSet<TimeRegistration> TimeRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Specify the table name for WorkLog
            modelBuilder.Entity<TimeRegistration>().ToTable("TimeRegistration");
        }
    }
}

