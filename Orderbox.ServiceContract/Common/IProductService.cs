using Framework.ServiceContract;
using Orderbox.Dto.Common;

namespace Orderbox.ServiceContract.Common
{
    public interface IProductService : IBaseTenantService<ProductDto, ulong>
    { }
}
