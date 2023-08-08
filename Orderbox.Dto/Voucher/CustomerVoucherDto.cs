using Framework.Dto;
using Orderbox.Dto.Common;

namespace Orderbox.Dto.Voucher
{
    public class CustomerVoucherDto : AuditableDto<ulong>
    {
        public ulong CustomerId { get; set; }

        public ulong VoucherId { get; set; }

        public CustomerDto Customer { get; set; }

        public VoucherDto Voucher { get; set; }
    }
}
