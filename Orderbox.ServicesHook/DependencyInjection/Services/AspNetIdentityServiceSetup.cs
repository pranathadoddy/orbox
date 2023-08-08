using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Authentication;
using Orderbox.ServiceContract.Authentication;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class AspNetIdentityServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IRoleService, RoleService>();
            service.AddScoped<IUserService, UserService>();
        }
    }
}
