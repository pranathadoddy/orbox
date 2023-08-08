using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orderbox.Core.Extensions;
using Orderbox.DataAccess.Application;
using Orderbox.DataAccess.Authentication;
using Orderbox.Dto.Authentication;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy;
using Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy.Store;
using Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy.Strategy;
using Orderbox.Mvc.Infrastructure.ServerUtility.Routing;
using Orderbox.ServicesHook.AutoMapper;
using Orderbox.ServicesHook.DependencyInjection;
using StackExchange.Redis;
using System;


namespace Orderbox.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConnectionString = this.Configuration.GetConnectionString("DefaultConnection");

            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            if (!WebHostEnvironment.IsDevelopment())
            {
                var redisConnectionString = this.Configuration.GetConnectionString("RedisConnection");
                var applicationName = this.Configuration.GetValue<string>("Application:Name");

                var redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
                services.AddDataProtection()
                    .SetApplicationName(applicationName)
                    .PersistKeysToStackExchangeRedis(redisConnection, applicationName);
            }

            services.AddDbContext<OrderboxContext>(context => context.UseMySql(databaseConnectionString, ServerVersion.AutoDetect(databaseConnectionString)));
            services
                .AddMultiTenancy()
                .WithResolutionStrategy<HostResolutionStrategy>()
                .WithStore<DatabaseTenantStore>();

            services.AddDbContext<AuthenticationContext>(options =>
                options.UseMySql(databaseConnectionString, ServerVersion.AutoDetect(databaseConnectionString)));

            services.AddIdentity<ApplicationUserDto, ApplicationRoleDto>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<AuthenticationContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 3;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                // If the LoginPath isn't set, ASP.NET Core defaults 
                // the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults 
                // the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/Login";
                options.SlidingExpiration = true;
            });

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUserDto>, CustomClaimsPrincipalFactory>();

            services.AddScoped<TenantRouteTransformer>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddHttpClient();

            services.AddElmah(options =>
            {
                options.OnPermissionCheck = context => context.User.Identity.IsAuthenticated && context.User.IsAdministrator();
                options.ConnectionString = databaseConnectionString;
                options.SqlServerDatabaseTableName = "ELMAH_Error";
            });

            Bootstrapper.SetupRepositories(services);
            Bootstrapper.SetupServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Home/Error?code={0}");
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseElmah();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDynamicControllerRoute<TenantRouteTransformer>("{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "area", pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
