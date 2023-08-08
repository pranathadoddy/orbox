using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orderbox.Api.Infrastructure.AuthenticationHandler;
using Orderbox.DataAccess.Application;
using Orderbox.DataAccess.Authentication;
using Orderbox.ServicesHook.AutoMapper;
using Orderbox.ServicesHook.DependencyInjection;

namespace OrderBox.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var databaseConnectionString = this.Configuration.GetConnectionString("DefaultConnection");

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                    options
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                );
            });
            services.AddHttpClient();
            services.AddDbContext<OrderboxContext>(context => context.UseMySql(databaseConnectionString, ServerVersion.AutoDetect(databaseConnectionString), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));
            services.AddDbContext<AuthenticationContext>(options => options.UseMySql(databaseConnectionString, ServerVersion.AutoDetect(databaseConnectionString), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<GoogleJwtAuthenticationScheme, GoogleJwtAuthenticationHandler>("Google", null);
            services.AddAutoMapper(typeof(AutoMapperProfile));

            BootstrapperApi.SetupRepositories(services);
            BootstrapperApi.SetupServices(services);
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowOrigin");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
