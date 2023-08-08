using Orderbox.Dto.Common;
using Orderbox.Dto.Transaction;
using System.Collections.Generic;

namespace Orderbox.ServiceContract.Common.Request
{
    public class CreateOrderRequest
    {
        public OrderDto OrderDto { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
