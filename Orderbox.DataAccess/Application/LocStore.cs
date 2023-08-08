using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class LocStore
    {
        public LocStore()
        {
            ComProductStores = new HashSet<ComProductStore>();
        }

        public ulong Id { get; set; }
        public ulong TenantId { get; set; }
        public ulong CityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MapUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual LocCity City { get; set; }
        public virtual ComTenant Tenant { get; set; }
        public virtual ICollection<ComProductStore> ComProductStores { get; set; }
    }
}
