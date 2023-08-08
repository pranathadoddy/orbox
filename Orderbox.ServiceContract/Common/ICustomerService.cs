using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Common;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Common
{
    public interface ICustomerService : IBaseService<CustomerDto, ulong>
    {
        Task<GenericResponse<CustomerDto>> ReadByCustomerIdAsync(GenericRequest<string> request);
    }
}
