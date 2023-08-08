using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;
using Orderbox.ServiceContract.Common.Request;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Common
{
    public interface ITenantService : IBaseService<TenantDto, ulong>
    {
        Task<GenericResponse<TenantDto>> ReadByUserIdAsync(GenericRequest<string> request);

        Task<GenericResponse<bool>> IsDomainExistAsync(IsDomainExistRequest request);
    }
}
