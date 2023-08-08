using Framework.Dto;
using System;

namespace Orderbox.Dto.Voucher
{
    public class VoucherDto : AuditableDto<ulong>
    {
        public ulong OrderItemId { get; set; }

        public string VoucherCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TermAndCondition { get; set; }

        public string RedeemMethod { get; set; }

        public string Status { get; set; }

        public DateTime RedeemDate { get; set; }

        public ulong? RedeemStoreId { get; set; }

        public DateTime ValidStartDate { get; set; }

        public DateTime ValidEndDate { get; set; }

        public CustomerVoucherDto CustomerVoucher { get; set; }
    }
}
