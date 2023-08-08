using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class ProductImageDto : TenantAuditableDto<ulong>
    {
        public ulong ProductId { get; set; }

        public string FileName { get; set; }

        public bool IsPrimary { get; set; }
    }
}
