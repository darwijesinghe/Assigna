using Domain.Classes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Domain.Data
{
    /// <summary>
    /// Data context class.
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Tables -------------------------------------
        public DbSet<Category> Category   { get; set; }
        public DbSet<Users> Users         { get; set; }
        public DbSet<Tasks> Tasks         { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Default values
            modelBuilder.Entity<Category>()
            .Property(b => b.InsertDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Users>()
            .Property(b => b.InsertDate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Tasks>()
            .Property(b => b.InsertDate).HasDefaultValueSql("getdate()");

            // Init data seeding

            // Categories
            var cats = new List<Category>()
            {
               new(){CatId = 1, CatName = "Team Task"},
               new(){CatId = 2, CatName = "Individual Task"},
               new(){CatId = 3, CatName = "Home Task"},
               new(){CatId = 4, CatName = "Finance Task"},
               new(){CatId = 5, CatName = "Client Task"},
               new(){CatId = 6, CatName = "Reasearch Task"},
            };

            // Add seed data
            modelBuilder.Entity<Category>().HasData(cats);

            base.OnModelCreating(modelBuilder);
        }
    }
}