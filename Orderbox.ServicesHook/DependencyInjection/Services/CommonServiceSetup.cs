using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Common;
using Orderbox.ServiceContract.Common;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class CommonServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IAgencyCategoryService, AgencyCategoryService>();
            service.AddScoped<IAgencyService, AgencyService>();
            service.AddScoped<IAgentService, AgentService>();
            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<ICustomerService, CustomerService>();
            service.AddScoped<IPaymentService, PaymentService>();
            service.AddScoped<IProductAgencyCategoryService, ProductAgencyCategoryService>();
            service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IProductImageService, ProductImageService>();
            service.AddScoped<IProductStoreService, ProductStoreService>();
            service.AddScoped<IRegistrationService, RegistrationService>();
            service.AddScoped<ITenantService, TenantService>();
            service.AddScoped<ITenantPostNotificationTokenService, TenantPostNotificationTokenService>();
        }
    }
}
