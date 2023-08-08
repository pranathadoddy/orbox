using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Authentication;
using Orderbox.ServiceContract.Authentication.Request;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Authentication
{
    public interface IUserService
    {
        Task<GenericResponse<ApplicationUserDto>> InsertAsync(UserRequest request);

        Task<GenericResponse<ApplicationUserDto>> ReadByUserNameAsync(GenericRequest<string> request);

        Task<GenericResponse<ApplicationUserDto>> UpdateAsync(UserRequest request);

        Task<BasicResponse> DeleteAsync(GenericRequest<string> request);

        Task<GenericResponse<ApplicationUserDto>> ReadByUserIdAsync(GenericRequest<string> request);

        Task<BasicResponse> SendResetPasswordEmailAsync(GenericWithEmailRequest<string> request);

        Task<BasicResponse> ResetPasswordAsync(ResetPasswordRequest request);

        Task<BasicResponse> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
