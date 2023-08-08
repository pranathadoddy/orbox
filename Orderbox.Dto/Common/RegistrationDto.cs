using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class RegistrationDto : AuditableDto<ulong>
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
