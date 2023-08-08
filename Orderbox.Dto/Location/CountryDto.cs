using Framework.Dto;

namespace Orderbox.Dto.Location
{
    public class CountryDto : AuditableDto<ulong>
    {
        public ulong AgencyId { get; set; }

        public string Name { get; set; }
    }
}
