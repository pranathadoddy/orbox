using Framework.RepositoryContract;
using Orderbox.Dto.Transaction;
using System.Collections.Generic;

namespace Orderbox.RepositoryContract.Transaction
{
    public interface IOrderItemRepository : IBaseTenantRepository<OrderItemDto>
    {
        void BulkInsertAsync(ulong orderId, ICollection<OrderItemDto> orderItemDtos);
    }
}
