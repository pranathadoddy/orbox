using System.Threading.Tasks;
using Framework.RepositoryContract;
using Orderbox.Dto.Common;

namespace Orderbox.RepositoryContract.Common
{
    public interface IProductAgencyCategoryRepository : IBaseRepository<ProductAgencyCategoryDto>
    {
        Task DeleteByProductIdAsync(ulong productId);
    }
}
