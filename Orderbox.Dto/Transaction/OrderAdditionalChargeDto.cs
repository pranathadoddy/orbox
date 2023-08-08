using Framework.Dto;

namespace Orderbox.Dto.Transaction
{
    public class OrderAdditionalChargeDto : TenantAuditableDto<ulong>
    {
        public ulong OrderId { get; set; }

        public string Name { get; set; }
        
        public decimal Amount { get; set; }
    }
}
