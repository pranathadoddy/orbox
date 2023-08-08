using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class TenantPushNotificationTokenDto: TenantAuditableDto<ulong>
    {
        public string Token { get; set; }
    }
}
