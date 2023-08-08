using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComProduct
    {
        public ComProduct()
        {
            ComProductAgencyCategories = new HashSet<ComProductAgencyCategory>();
            ComProductImages = new HashSet<ComProductImage>();
            ComProductStores = new HashSet<ComProductStore>();
            ComProductSubCategories = new HashSet<ComProductSubCategory>();
        }

        public ulong Id { get; set; }
        public ulong TenantId { get; set; }
        public ulong CategoryId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public decimal? Discount { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? ValidPeriodStart { get; set; }
        public DateTime? ValidPeriodEnd { get; set; }
        public double? Commission { get; set; }
        public string TermAndCondition { get; set; }
        public string RedeemMethod { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComCategory Category { get; set; }
        public virtual ComTenant Tenant { get; set; }
        public virtual ICollection<ComProductAgencyCategory> ComProductAgencyCategories { get; set; }
        public virtual ICollection<ComProductImage> ComProductImages { get; set; }
        public virtual ICollection<ComProductStore> ComProductStores { get; set; }
        public virtual ICollection<ComProductSubCategory> ComProductSubCategories { get; set; }
    }
}
