using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComSubCategory
    {
        public ComSubCategory()
        {
            ComProductSubCategories = new HashSet<ComProductSubCategory>();
        }

        public ulong Id { get; set; }
        public ulong TenantId { get; set; }
        public ulong CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComCategory Category { get; set; }
        public virtual ComTenant Tenant { get; set; }
        public virtual ICollection<ComProductSubCategory> ComProductSubCategories { get; set; }
    }
}
