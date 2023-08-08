using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComCategory
    {
        public ComCategory()
        {
            ComProducts = new HashSet<ComProduct>();
            ComSubCategories = new HashSet<ComSubCategory>();
        }

        public ulong Id { get; set; }
        public ulong TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComTenant Tenant { get; set; }
        public virtual ICollection<ComProduct> ComProducts { get; set; }
        public virtual ICollection<ComSubCategory> ComSubCategories { get; set; }
    }
}
