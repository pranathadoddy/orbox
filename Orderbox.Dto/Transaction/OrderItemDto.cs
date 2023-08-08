using System;
using Framework.Dto;

namespace Orderbox.Dto.Transaction
{
    public class OrderItemDto : TenantAuditableDto<ulong>
    {
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

        public decimal Discount { get; set; }

        public decimal UnitPrice { get; set; }

        public double Commission { get; set; }

        public int Quantity { get; set; }

        public string Note { get; set; }

        public string AvailableRedeemStores { get; set; }

        public decimal ExtTotalPrice 
        { 
            get { return this.Quantity * (this.UnitPrice - (this.Discount * this.UnitPrice / 100)); }  
        }

        public string ExtProductImageUrl { get; set; }
    }
}
