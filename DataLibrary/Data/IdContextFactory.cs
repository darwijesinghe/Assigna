using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataLibrary.Data
{
    public class IdContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            // build config
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../UserInterface"))
            .AddJsonFile("appsettings.json")
            .Build();

            // get connection string
            var builder = new DbContextOptionsBuilder<IdentityContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new IdentityContext(builder.Options);
        }
    }
}