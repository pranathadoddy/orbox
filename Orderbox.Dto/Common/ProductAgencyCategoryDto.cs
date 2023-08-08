using Framework.Dto;

namespace Orderbox.Dto.Common
{
    public  class ProductAgencyCategoryDto : TenantAuditableDto<ulong>
    {
        public ulong AgencyId { get; set; }

        public ulong AgencyCategoryId { get; set; }

        public ulong ProductId { get; set; }

        public string AgencyCategoryName { get; set; }

        public ProductDto Product { get; set; }
    }
}
