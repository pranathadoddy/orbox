using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class VchVoucher
    {
        public VchVoucher()
        {
            VchCustomerVouchers = new HashSet<VchCustomerVoucher>();
        }

        public ulong Id { get; set; }
        public ulong OrderItemId { get; set; }
        public string VoucherCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TermAndCondition { get; set; }
        public string RedeemMethod { get; set; }
        public string Status { get; set; }
        public DateTime? RedeemDate { get; set; }
        public ulong? RedeemStoreId { get; set; }
        public DateTime? ValidStartDate { get; set; }
        public DateTime? ValidEndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual TrxOrderItem OrderItem { get; set; }
        public virtual ICollection<VchCustomerVoucher> VchCustomerVouchers { get; set; }
    }
}
