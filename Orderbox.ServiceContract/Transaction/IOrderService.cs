using Framework.ServiceContract;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Transaction;
using System.Threading.Tasks;

namespace Orderbox.ServiceContract.Transaction
{
    public interface IOrderService : IBaseTenantService<OrderDto, ulong>
    {
        Task<GenericResponse<OrderDto>> GetLatestOrderOfTheCurrentTenantAsync(GenericRequest<ulong> request);

        Task<GenericResponse<OrderDto>> ReadByIdAsync(GenericWithCustomerRequest<ulong> request);
    }
}
