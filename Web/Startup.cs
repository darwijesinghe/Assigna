using Domain.Classes;
using Domain.Data;
using Domain.Repositories.Classes;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Services.Classes;
using Services.Interfaces;
using System;
using UserInterface.Services;

namespace UserInterface
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Database options
            services.ConfigureOptions<DatabaseOptionsSetup>();

            // Database service
            services.AddDbContext<DataContext>((serviceprovider, option) =>
            {
                // gets database option values
                var options = serviceprovider.GetService<IOptions<DatabaseOptions>>()!.Value;

                option.UseSqlServer(options.ConnectionString, action =>
                {
                    action.MigrationsAssembly("Domain");
                    action.CommandTimeout(options.CommandTimeout);
                });
            });

            // Identity databse options
            services.AddDbContext<IdentityContext>((serviceprovider, option) =>
            {
                // gets database option values
                var options = serviceprovider.GetService<IOptions<DatabaseOptions>>()!.Value;

                option.UseSqlServer(options.ConnectionString, action =>
                {
                    action.MigrationsAssembly("Domain");
                    action.CommandTimeout(options.CommandTimeout);
                });
            });

            // Add identity service
            services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                // password options
                option.Password.RequiredLength         = 4;
                option.Password.RequireDigit           = true;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase       = false;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            // Cookie config
            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath         = "/account/signin";
                option.ExpireTimeSpan    = TimeSpan.FromMinutes(10);
                option.SlidingExpiration = true;
            });

            // External logins
            services.AddAuthentication().AddGoogle(option =>
            {
                option.ClientId     = _configuration["Authentication:Google:ClientId"];
                option.ClientSecret = _configuration["Authentication:Google:ClientSecret"];
            });

            // Get mail service setting values
            var mailconfig = _configuration.GetSection("MailConfigurations").Get<MailConfigurations>();
            services.AddSingleton(mailconfig);

            // Token life span
            services.Configure<DataProtectionTokenProviderOptions>(option =>
            {
                option.TokenLifespan = TimeSpan.FromHours(2);
            });

            // Register repositories
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Register services
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();

            // Mail service
            services.AddScoped<IMailService, MailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Unhandle error handeling
                app.UseExceptionHandler("/error");

                // HTTP status error handling
                app.UseStatusCodePagesWithReExecute("/error/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Aunthentication
            // Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Start route
                endpoints.MapControllerRoute(
                    name: "account",
                    pattern: "{action}/{id?}",
                    defaults: new { controller = "Account", action = "SignIn" });

                // Task route
                endpoints.MapControllerRoute(
                    name: "task",
                    pattern: "{action}/{id?}",
                    defaults: new { controller = "Task", action = "Tasks" });

                // Default route
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=SignIn}/{id?}");
            });

            // Apply pending migrations
            ApplyMigrations(app);
        }

        /// <summary>
        /// Applies any pending database migrations at application startup to ensure
        /// the database schema is up-to-date with the latest changes
        /// </summary>
        /// <param name="app">The application builder instance used to configure the request pipeline</param>
        private static void ApplyMigrations(IApplicationBuilder app)
        {
            // Apply pending migrations automatically
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DataContext>();
                dbContext.Database.Migrate();
            }

            // Apply pending identity migrations automatically
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<IdentityContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}