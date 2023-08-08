using Framework.Service;
using Orderbox.Dto.Location;
using Orderbox.RepositoryContract.Location;
using Orderbox.ServiceContract.Location;

namespace Orderbox.Service.Location
{
    public class CountryService : BaseService<CountryDto, ulong, ICountryRepository>, ICountryService
    {
        public CountryService(ICountryRepository repository) : base(repository)
        {
            
        }
    }
}