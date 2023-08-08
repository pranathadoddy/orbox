using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Utility;
using Orderbox.ServiceContract.Utility;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class UtilityServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IRecaptchaValidator, RecaptchaValidator>();
        }
    }
}