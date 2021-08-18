using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenHackathonWeb.Helpers;
using System;
using System.Linq;

namespace OpenHackathonWeb.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HackathonDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<HackathonDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                context.Users.AddRange(
                    new Users
                    {
                        FirstName = "Jon",
                        LastName = "Smith",
                        Email = "jon@email.com",
                        Password = "123456",
                        WalletAddress = "PEpYnf7Zv5u4pTDGmvYPT2vktpop2VeCqj",
                        UserRole = (int)UserRoles.Owner
                    },

                    new Users
                    {
                        FirstName = "William",
                        LastName = "James",
                        Email = "will@email.com",
                        Password = "123456",
                        WalletAddress = "PQEf8KCNpLgzGA17m61LSKxwF3bwG1XYQ7",
                        UserRole = (int)UserRoles.HackathonManager
                    },

                    new Users
                    {
                        FirstName = "Benjamin",
                        LastName = "State",
                        Email = "ben@email.com",
                        Password = "123456",
                        WalletAddress = "PJs85PCxHddAXf4WSuG8MoPEGxSmEedTjG",
                        UserRole = (int)UserRoles.RegisteredMember
                    },

                    new Users
                    {
                        FirstName = "Ava",
                        LastName = "Jons",
                        Email = "ava@email.com",
                        Password = "123456",
                        WalletAddress = "PKuy4dacz3JYnEJiB43e1pNVHdHJscehEs",
                        UserRole = (int)UserRoles.RegisteredMember
                    },

                    new Users
                    {
                        FirstName = "Olivia",
                        LastName = "Gates",
                        Email = "oliva@email.com",
                        Password = "123456",
                        WalletAddress = "P95hHtFFtjvoKYFoFWAWPa1mmd48eqTVrc",
                        UserRole = (int)UserRoles.RegisteredMember
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
