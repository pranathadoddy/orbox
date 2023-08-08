using Framework.Core.Resources;
using Framework.Service;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Orderbox.Dto.Transaction;
using Orderbox.RepositoryContract.Transaction;
using Orderbox.ServiceContract.Transaction;
using System.Threading.Tasks;

namespace Orderbox.Service.Common
{
    public class OrderService : BaseTenantService<OrderDto, ulong, IOrderRepository>, IOrderService
    {
        public OrderService(IOrderRepository repository) : base(repository)
        {
            
        }

        public async Task<GenericResponse<OrderDto>> GetLatestOrderOfTheCurrentTenantAsync(GenericRequest<ulong> request)
        {
            var response = new GenericResponse<OrderDto>();

            response.Data = await this._repository.GetLatestOrderOfTheCurrentTenantAsync(request.Data);
                        
            if(response.Data == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
            }

            return response;
        }

        public async Task<GenericResponse<OrderDto>> ReadByIdAsync(GenericWithCustomerRequest<ulong> request)
        {
            var response = new GenericResponse<OrderDto>();

            response.Data = await this._repository.ReadByIdAsync(request.CustomerId, request.Data);

            if (response.Data == null)
            {
                response.AddErrorMessage(GeneralResource.Item_NotFound);
            }

            return response;
        }
    }
}
