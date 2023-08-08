using Framework.Service;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class SubCategoryService : BaseTenantService<SubCategoryDto, ulong, ISubCategoryRepository>, ISubCategoryService
    {
        #region Constructor

        public SubCategoryService(ISubCategoryRepository repository) : base(repository)
        {
        }

        #endregion
    }
}
