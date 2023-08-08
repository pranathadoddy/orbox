using Orderbox.DataAccess.Application;
using Orderbox.Dto.Common;

namespace Orderbox.ServicesHook.AutoMapper
{
    public class CommonEntityMapperSetup
    {
        public static void Initialize(AutoMapperProfile profile)
        {
            profile.CreateMap<ComAgency, AgencyDto>().ReverseMap();
            profile.CreateMap<ComAgent, AgentDto>().ReverseMap();
            profile.CreateMap<ComAgencyCategory, AgencyCategoryDto>().ReverseMap();
            profile.CreateMap<ComCategory, CategoryDto>().ReverseMap();
            profile.CreateMap<ComCustomer, CustomerDto>().ReverseMap();
            profile.CreateMap<ComPayment, PaymentDto>().ReverseMap();
            profile.CreateMap<ComProduct, ProductDto>().ReverseMap();
            profile.CreateMap<ComProductStore, ProductStoreDto>().ReverseMap();
            profile.CreateMap<ComProductAgencyCategory, ProductAgencyCategoryDto>().ReverseMap();
            profile.CreateMap<ComProductImage, ProductImageDto>().ReverseMap();
            profile.CreateMap<ComRegistration, RegistrationDto>().ReverseMap();
            profile.CreateMap<ComSubCategory, SubCategoryDto>().ReverseMap();
            profile.CreateMap<ComTenant, TenantDto>().ReverseMap();
            profile.CreateMap<ComTenantPushNotificationToken, TenantPushNotificationTokenDto>().ReverseMap();
        }
    }
}