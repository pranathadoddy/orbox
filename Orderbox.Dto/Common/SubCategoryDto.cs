using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class SubCategoryDto : TenantAuditableDto<ulong>
    {
        public ulong CategoryId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}
