using Framework.Service;
using Orderbox.Dto.Location;
using Orderbox.RepositoryContract.Location;
using Orderbox.ServiceContract.Location;

namespace Orderbox.Service.Location
{
    public class StoreService : BaseService<StoreDto, ulong, IStoreRepository>, IStoreService
    {
        public StoreService(IStoreRepository repository) : base(repository)
        {
            
        }
    }
}