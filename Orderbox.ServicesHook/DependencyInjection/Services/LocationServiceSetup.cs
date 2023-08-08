using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Location;
using Orderbox.ServiceContract.Location;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class LocationServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<ICountryService, CountryService>();
            service.AddScoped<ICityService, CityService>();
            service.AddScoped<IStoreService, StoreService>();
        }
    }
}