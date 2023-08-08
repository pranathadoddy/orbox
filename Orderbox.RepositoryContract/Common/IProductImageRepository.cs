using Framework.RepositoryContract;
using Orderbox.Dto.Common;
using System.Threading.Tasks;

namespace Orderbox.RepositoryContract.Common
{
    public interface IProductImageRepository : IBaseTenantRepository<ProductImageDto>
    {
        Task<ProductImageDto> SetPrimaryAsync(ProductImageDto dto);
    }
}
