using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assignment.Framework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Assignment.Framework.Helpers
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DataContext(
                serviceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
            {
                if (context.RegisteredUsers.Any())
                {
                    return;   // Data was already seeded
                }

                context.RegisteredUsers.AddRange(
                    new RegisteredUser
                    {
                        UserId = "Sam32",
                        UserPassword = "Sam32",
                        FirstName = "Sam",
                        LastName = "James",
                        Email = "SamJames@gmail.com",
                        IsActive = true,
                        Roles = "Admin"
                    },
                    new RegisteredUser
                    {
                        UserId = "Shiv23",
                        UserPassword = "Shiv63",
                        FirstName = "Shiv",
                        LastName = "Bhardwaj",
                        Email = "ShivBhardwaj@gmail.com",
                        IsActive = true,
                        Roles = "User"
                    }
                    );
                context.SaveChanges();

            }
        }
    }
}
