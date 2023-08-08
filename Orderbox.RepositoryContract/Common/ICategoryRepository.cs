using Framework.RepositoryContract;
using Orderbox.Dto.Common;

namespace Orderbox.RepositoryContract.Common
{
    public interface ICategoryRepository : IBaseTenantRepository<CategoryDto>
    { }
}
