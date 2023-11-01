using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DataLibrary.Data
{
    public class DatabaseOptions
    {
        public int CommandTimeout { get; set; }
        public string ConnectionString { get; set; } = string.Empty;
        public bool EnableDetailedErrors { get; set; }
        public int MaxRetryCount { get; set; }
    }

    public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
    {        
        // getting values from the appsettings.json file
        // for the setup database options
        private readonly IConfiguration _configuration;

        public DatabaseOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DatabaseOptions options)
        {
            var connection = _configuration.GetConnectionString("DefaultConnection");
            options.ConnectionString = connection;

            // bind rest of values
            _configuration.GetSection("DatabaseOptions").Bind(options);
        }
    }
}