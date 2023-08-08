using Orderbox.DataAccess.Application;
using Orderbox.Dto.Voucher;

namespace Orderbox.ServicesHook.AutoMapper
{
    public class VoucherEntityMapperSetup
    {
        public static void Initialize(AutoMapperProfile profile)
        {
            profile.CreateMap<VchCustomerVoucher, CustomerVoucherDto>().ReverseMap();
            profile.CreateMap<VchVoucher, VoucherDto>().ReverseMap();            
        }
    }
}
