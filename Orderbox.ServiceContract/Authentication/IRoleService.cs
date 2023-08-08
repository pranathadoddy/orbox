using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Authentication
{
    public interface IRoleService
    {
        Task<GenericResponse<ApplicationRoleDto>> InsertAsync(GenericRequest<string> request);

        Task<GenericResponse<ApplicationRoleDto>> ReadAsync(GenericRequest<string> request);

        Task<GenericResponse<List<ApplicationRoleDto>>> AllRoleAsync();

        Task<BasicResponse> DeleteAsync(GenericRequest<string> request);
    }
}
