using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComProductSubCategory
    {
        public ulong Id { get; set; }
        public ulong TenantId { get; set; }
        public ulong ProductId { get; set; }
        public ulong SubCategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComProduct Product { get; set; }
        public virtual ComSubCategory SubCategory { get; set; }
        public virtual ComTenant Tenant { get; set; }
    }
}
