using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public class CustomerDto : AuditableDto<ulong>
    {
        public string AuthType { get; set; }

        public string ExternalId { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string ProfilePicture { get; set; }
    }
}
