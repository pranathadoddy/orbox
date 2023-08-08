using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class AgentDto : AuditableDto<ulong>
    {
        public string Email { get; set; }

        public ulong AgencyId { get; set; }

        public string UserId { get; set; }

        public string Privilege { get; set; }

        public string Status { get; set; }
    }
}
