using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComPayment
    {
        public ulong Id { get; set; }
        public ulong TenantId { get; set; }
        public string PaymentOptionCode { get; set; }
        public string ProviderName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Description { get; set; }
        public string ApiKey { get; set; }
        public string WebhookValidationSecret { get; set; }
        public int? ActiveDurationInMinutes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComTenant Tenant { get; set; }
    }
}
