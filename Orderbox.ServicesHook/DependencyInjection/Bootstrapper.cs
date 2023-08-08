using Microsoft.Extensions.DependencyInjection;
using Orderbox.ServicesHook.DependencyInjection.Repositories;
using Orderbox.ServicesHook.DependencyInjection.Services;

namespace Orderbox.ServicesHook.DependencyInjection
{
    public class Bootstrapper
    {
        public static void SetupRepositories(IServiceCollection service)
        {
            CommonRepositorySetup.Initialize(service);
            LocationRepositorySetup.Initialize(service);
            TransactionRepositorySetup.Initialize(service);
            VoucherRepositorySetup.Initialize(service);
        }

        public static void SetupServices(IServiceCollection service)
        {
            AspNetIdentityServiceSetup.Initialize(service);
            AssetsManagerServiceSetup.Initialize(service);
            CommonServiceSetup.Initialize(service);
            EmailManagerServiceSetup.Initialize(service);
            LocationServiceSetup.Initialize(service);
            PaymentServiceSetup.Initialize(service);
            PushNotificationServiceSetup.Initialize(service);
            ReportServiceSetup.Initialize(service);
            SeedServiceSetup.Initialize(service);
            TransactionServiceSetup.Initialize(service);
            UtilityServiceSetup.Initialize(service);
            VoucherServiceSetup.Initialize(service);
        }
    }
}
