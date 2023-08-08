using Framework.ServiceContract;
using Orderbox.Dto.Common;

namespace Orderbox.ServiceContract.Common
{
    public interface ISubCategoryService : IBaseTenantService<SubCategoryDto, ulong>
    { }
}
