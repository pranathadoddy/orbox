using Microsoft.Extensions.DependencyInjection;
using Orderbox.Repository.Location;
using Orderbox.RepositoryContract.Location;

namespace Orderbox.ServicesHook.DependencyInjection.Repositories
{
    public class LocationRepositorySetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<ICountryRepository, CountryRepository>();
            service.AddScoped<ICityRepository, CityRepository>();
            service.AddScoped<IStoreRepository, StoreRepository>();
        }
    }
}
