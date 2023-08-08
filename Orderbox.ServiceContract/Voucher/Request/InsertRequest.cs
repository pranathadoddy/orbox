using Framework.ServiceContract.Request;
using Orderbox.Dto.Transaction;
using Orderbox.Dto.Voucher;

namespace Orderbox.ServiceContract.Voucher.Request
{
    public class InsertRequest : GenericRequest<VoucherDto>
    {
        public OrderDto OrderDto { get; set; }
    }
}
