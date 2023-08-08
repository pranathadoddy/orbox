using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class TrxOrderAdditionalCharge
    {
        public ulong Id { get; set; }
        public ulong TenantId { get; set; }
        public ulong OrderId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual TrxOrder Order { get; set; }
        public virtual ComTenant Tenant { get; set; }
    }
}
