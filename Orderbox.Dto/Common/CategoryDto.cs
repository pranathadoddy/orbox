using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class CategoryDto : TenantAuditableDto<ulong>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
