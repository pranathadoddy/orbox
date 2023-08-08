using System.Threading.Tasks;
using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;

namespace Orderbox.ServiceContract.Common
{
    public interface IProductStoreService : IBaseTenantService<ProductStoreDto, ulong>
    {
        Task<BasicResponse> DeleteByProductIdAsync(GenericRequest<ulong> request);
    }
}
