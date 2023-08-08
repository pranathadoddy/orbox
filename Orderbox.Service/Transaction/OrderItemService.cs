using Framework.Service;
using Orderbox.Dto.Transaction;
using Orderbox.RepositoryContract.Transaction;
using Orderbox.ServiceContract.Transaction;

namespace Orderbox.Service.Common
{
    public class OrderItemService : BaseTenantService<OrderItemDto, ulong, IOrderItemRepository>, IOrderItemService
    {
        public OrderItemService(IOrderItemRepository repository) : base(repository)
        {
        }
    }
}
