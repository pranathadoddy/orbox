using Microsoft.Extensions.DependencyInjection;
using Orderbox.Repository.Common;
using Orderbox.RepositoryContract.Common;

namespace Orderbox.ServicesHook.DependencyInjection.Repositories
{
    public class CommonRepositorySetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IAgencyCategoryRepository, AgencyCategoryRepository>();
            service.AddScoped<IAgencyRepository, AgencyRepository>();
            service.AddScoped<IAgentRepository, AgentRepository>();
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            service.AddScoped<ICustomerRepository, CustomerRepository>();
            service.AddScoped<IReportRepository, ReportRepository>();
            service.AddScoped<IPaymentRepository, PaymentRepository>();
            service.AddScoped<IProductAgencyCategoryRepository, ProductAgencyCategoryRepository>();
            service.AddScoped<IProductRepository, ProductRepository>();
            service.AddScoped<IProductImageRepository, ProductImageRepository>();
            service.AddScoped<IProductStoreRepository, ProductStoreRepository>();
            service.AddScoped<IRegistrationRepository, RegistrationRepository>();
            service.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            service.AddScoped<ITenantRepository, TenantRepository>();
            service.AddScoped<ITenantPushNotificationTokenRepository, TenantPushNotificationTokenRepository>();
        }
    }
}
