using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComTenantPushNotificationToken
    {
        public ulong Id { get; set; }
        public ulong TenantId { get; set; }
        public string Token { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComTenant Tenant { get; set; }
    }
}
