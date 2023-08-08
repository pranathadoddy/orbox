using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Seeds;
using Orderbox.ServiceContract.Seeds;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class SeedServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IRoleSeed, RoleSeed>();
            service.AddScoped<IUserSeed, UserSeed>();
        }
    }
}
