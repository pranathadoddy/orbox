using Framework.Dto;

namespace Orderbox.Dto.Location
{
    public class StoreDto : AuditableDto<ulong>
    {
        public ulong TenantId { get; set; }

        public ulong CityId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string MapUrl { get; set; }

        public CityDto City { get; set; }
    }
}
