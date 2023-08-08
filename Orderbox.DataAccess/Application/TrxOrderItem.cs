using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class TrxOrderItem
    {
        public TrxOrderItem()
        {
            VchVouchers = new HashSet<VchVoucher>();
        }

        public ulong Id { get; set; }
        public ulong TenantId { get; set; }
        public ulong OrderId { get; set; }
        public ulong ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductUnit { get; set; }
        public DateTime? ValidEndDate { get; set; }
        public DateTime? ValidStartDate { get; set; }
        public string ProductRedeemMethod { get; set; }
        public string ProductTermAndCondition { get; set; }
        public string ProductDescription { get; set; }
        public string ProductType { get; set; }
        public decimal? Discount { get; set; }
        public decimal UnitPrice { get; set; }
        public double? Commission { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public string AvailableRedeemStores { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual TrxOrder Order { get; set; }
        public virtual ComTenant Tenant { get; set; }
        public virtual ICollection<VchVoucher> VchVouchers { get; set; }
    }
}
