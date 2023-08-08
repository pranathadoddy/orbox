using Framework.Service;
using Orderbox.Dto.Common;
using Orderbox.RepositoryContract.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.Service.Common
{
    public class ProductService : BaseTenantService<ProductDto, ulong, IProductRepository>, IProductService
    {
        #region Constructor

        public ProductService(IProductRepository repository) : base(repository)
        {
        }

        #endregion
    }
}
