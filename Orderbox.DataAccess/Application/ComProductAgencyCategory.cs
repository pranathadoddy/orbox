using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComProductAgencyCategory
    {
        public ulong Id { get; set; }
        public ulong AgencyId { get; set; }
        public ulong TenantId { get; set; }
        public ulong ProductId { get; set; }
        public ulong AgencyCategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComAgency Agency { get; set; }
        public virtual ComAgencyCategory AgencyCategory { get; set; }
        public virtual ComProduct Product { get; set; }
        public virtual ComTenant Tenant { get; set; }
    }
}
