using DataLibrary.Data;
using DataLibrary.Interfaces;
using DataLibrary.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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

            // database options
            services.ConfigureOptions<DatabaseOptionsSetup>();

            // database service
            services.AddDbContext<DataContext>((serviceprovider, option) =>
            {
                // get database option values
                var options = serviceprovider.GetService<IOptions<DatabaseOptions>>()!.Value;

                option.UseSqlServer(options.ConnectionString, action =>
                {
                    action.MigrationsAssembly("DataLibrary");
                    action.CommandTimeout(options.CommandTimeout);
                });
            });

            // identity databse options
            services.AddDbContext<IdentityContext>((serviceprovider, option) =>
            {
                // get database option values
                var options = serviceprovider.GetService<IOptions<DatabaseOptions>>()!.Value;

                option.UseSqlServer(options.ConnectionString, action =>
                {
                    action.MigrationsAssembly("DataLibrary");
                    action.CommandTimeout(options.CommandTimeout);
                });
            });

            // add identity service
            services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                // password options
                option.Password.RequiredLength = 4;
                option.Password.RequireDigit = true;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            // cookie config
            services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = "/account/signin";
                option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                option.SlidingExpiration = true;
            });

            // external logins
            services.AddAuthentication().AddGoogle(option =>
            {
                option.ClientId = _configuration["Authentication:Google:ClientId"];
                option.ClientSecret = _configuration["Authentication:Google:ClientSecret"];
            });

            // get mail service setting values
            var mailconfig = _configuration.GetSection("MailConfigurations").Get<MailConfigurations>();
            services.AddSingleton(mailconfig);

            // token life span
            services.Configure<DataProtectionTokenProviderOptions>(option =>
            {
                option.TokenLifespan = TimeSpan.FromHours(2);
            });

            // dependency services

            // database service
            services.AddScoped<IDataService, DataService>();
            // mail service
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
                // unhandle error handeling
                app.UseExceptionHandler("/error");

                // http status error handling
                app.UseStatusCodePagesWithReExecute("/error/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // aunthentication
            // authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // start route
                endpoints.MapControllerRoute(
                    name: "account",
                    pattern: "{action}/{id?}",
                    defaults: new { controller = "Account", action = "SignIn" });

                // task route
                endpoints.MapControllerRoute(
                    name: "task",
                    pattern: "{action}/{id?}",
                    defaults: new { controller = "Task", action = "Tasks" });

                // default route
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=SignIn}/{id?}");
            });
        }
    }
}