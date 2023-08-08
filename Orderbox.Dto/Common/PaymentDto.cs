using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class PaymentDto : TenantAuditableDto<ulong>
    {
        public string PaymentOptionCode { get; set; }

        public string ProviderName { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string Description { get; set; }

        public string ApiKey { get; set; }

        public string WebhookValidationSecret { get; set; }

        public int? ActiveDurationInMinutes { get; set; }
    }
}
