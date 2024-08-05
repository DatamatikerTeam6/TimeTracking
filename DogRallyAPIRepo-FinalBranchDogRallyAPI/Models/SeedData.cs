using DogRallyAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DogRallyAPI.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TimeTrackingDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TimeTrackingDbContext>>()))
            {
                context.Database.EnsureCreated();

                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Check if the DB has been seeded
                if (context.Timetrackings.Any() && context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                // Add roles and users
                string[] roles = new string[] { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!roleManager.RoleExistsAsync(role).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole(role)).Wait();
                    }
                }

                // Seed admin user
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@dogrally.com",
                    Email = "admin@dogrally.com",
                    EmailConfirmed = true
                };


                if (userManager.FindByEmailAsync(adminUser.Email).Result == null)
                {
                    var result = userManager.CreateAsync(adminUser, "Admin123!").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                    }
                }

                // Seed regular user
                var regularUser = new ApplicationUser
                {
                    UserName = "regular@dogrally.com",
                    Email = "regular@dogrally.com",
                    EmailConfirmed = true
                };

                if (userManager.FindByEmailAsync(regularUser.Email).Result == null)
                {
                    var result = userManager.CreateAsync(regularUser, "Regular123!").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(regularUser, "User").Wait();
                    }
                };
                    context.SaveChanges();
                }
            }
        }
    }

