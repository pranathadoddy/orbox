using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class AgencyCategoryDto : AuditableDto<ulong>
    {
        public ulong AgencyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public bool IsMainCategory { get; set; }
    }
}
