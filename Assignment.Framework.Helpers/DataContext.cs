using System;
using Assignment.Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Framework.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) 
            : base(options)
        {
           // LoadCategories();
        }

        private void LoadCategories()
        {
           
        }
        public DbSet<RegisteredUser> RegisteredUsers { get; set; }
    }
}
