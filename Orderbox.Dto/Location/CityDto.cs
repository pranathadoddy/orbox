using Framework.Dto;

namespace Orderbox.Dto.Location
{
    public class CityDto : AuditableDto<ulong>
    {
        public ulong AgencyId { get; set; }

        public ulong CountryId { get; set; }

        public string Name { get; set; }

        public CountryDto Country { get; set; }
    }
}
