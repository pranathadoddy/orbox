using Framework.Service;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class CategoryService : BaseTenantService<CategoryDto, ulong, ICategoryRepository>, ICategoryService
    {
        #region Constructor

        public CategoryService(ICategoryRepository repository) : base(repository)
        {
        }

        #endregion
    }
}
