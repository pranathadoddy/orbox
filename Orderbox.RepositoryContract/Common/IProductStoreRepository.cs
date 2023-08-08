using System.Threading.Tasks;
using Framework.RepositoryContract;
using Orderbox.Dto.Common;

namespace Orderbox.RepositoryContract.Common
{
    public interface IProductStoreRepository : IBaseTenantRepository<ProductStoreDto>
    {
        Task DeleteByProductIdAsync(ulong productId);
    }
}
