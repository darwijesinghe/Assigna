using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataLibrary.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            // build config
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../UserInterface"))
            .AddJsonFile("appsettings.json")
            .Build();

            // get connection string
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new DataContext(builder.Options);
        }
    }
}