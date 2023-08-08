using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Common
{
    public interface IRegistrationService : IBaseService<RegistrationDto, ulong>
    {
        Task<GenericResponse<RegistrationDto>> RegisterAsync(GenericWithEmailRequest<RegistrationDto> request);

        Task<GenericResponse<RegistrationDto>> ReadByEmailAndVerificationCodeAsync(GenericRequest<RegistrationDto> request);
    }
}
