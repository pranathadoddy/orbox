using Framework.Dto;
using System;
using System.Collections.Generic;

namespace Orderbox.Dto.Common
{
    public class ProductDto : TenantAuditableDto<ulong>
    {
        public ulong CategoryId { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public string Unit { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime ValidPeriodStart { get; set; }

        public DateTime ValidPeriodEnd { get; set; }

        public double Commission { get; set; }

        public string TermAndCondition { get; set; }

        public string RedeemMethod { get; set; }

        public List<ProductImageDto> ProductImages { get; set; }
    }
}
