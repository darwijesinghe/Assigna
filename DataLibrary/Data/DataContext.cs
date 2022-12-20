using DataLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataLibrary.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // tables
        public DbSet<Category> category { get; set; } = null!;

        public DbSet<Users> users { get; set; } = null!;
        public DbSet<Task> task { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // default values
            modelBuilder.Entity<Category>()
            .Property(b => b.insertdate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Users>()
            .Property(b => b.insertdate).HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Task>()
            .Property(b => b.insertdate).HasDefaultValueSql("getdate()");

            #region init data seed

            // category
            var cats = new List<Category>()
            {
               new(){cat_id = 1, cat_name = "Team Task"},
               new(){cat_id = 2, cat_name = "Individual Task"},
               new(){cat_id = 3, cat_name = "Home Task"},
               new(){cat_id = 4, cat_name = "Finance Task"},
               new(){cat_id = 5, cat_name = "Client Task"},
               new(){cat_id = 6, cat_name = "Reasearch Task"},
            };

            modelBuilder.Entity<Category>().HasData(cats);

            #endregion init data seed

            base.OnModelCreating(modelBuilder);
        }
    }
}