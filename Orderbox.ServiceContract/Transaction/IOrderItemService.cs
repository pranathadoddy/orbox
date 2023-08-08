using Framework.ServiceContract;
using Orderbox.Dto.Transaction;

namespace Orderbox.ServiceContract.Transaction
{
    public interface IOrderItemService : IBaseTenantService<OrderItemDto, ulong>
    {
    }
}
