using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class AgencyDto : AuditableDto<ulong>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
