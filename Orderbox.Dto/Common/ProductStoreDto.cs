using Framework.Dto;
using Orderbox.Dto.Location;

namespace Orderbox.Dto.Common
{
    public class ProductStoreDto : TenantAuditableDto<ulong>
    {
        public ulong ProductId { get; set; }

        public ulong StoreId { get; set; }

        public StoreDto Store { get; set; }
    }
}
