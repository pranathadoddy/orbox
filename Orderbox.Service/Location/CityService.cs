using Framework.Service;
using Orderbox.Dto.Location;
using Orderbox.RepositoryContract.Location;
using Orderbox.ServiceContract.Location;

namespace Orderbox.Service.Location
{
    public class CityService : BaseService<CityDto, ulong, ICityRepository>, ICityService
    {
        public CityService(ICityRepository repository) : base(repository)
        {
            
        }
    }
}