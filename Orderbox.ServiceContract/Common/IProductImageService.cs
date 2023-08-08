using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Common
{
    public interface IProductImageService: IBaseTenantService<ProductImageDto, ulong>
    {
        Task<GenericResponse<ProductImageDto>> SetPrimaryAsync(GenericRequest<ProductImageDto> request);
    }
}
