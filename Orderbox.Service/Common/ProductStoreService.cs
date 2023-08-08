using System.Threading.Tasks;
using Framework.Service;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class ProductStoreService : BaseTenantService<ProductStoreDto, ulong, IProductStoreRepository>, IProductStoreService
    {
        public ProductStoreService(IProductStoreRepository repository) : base(repository)
        {
        }

        public async Task<BasicResponse> DeleteByProductIdAsync(GenericRequest<ulong> request)
        {
            await this._repository.DeleteByProductIdAsync(request.Data);
            return new BasicResponse();
        }
    }
}
