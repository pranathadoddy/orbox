using Microsoft.Extensions.DependencyInjection;
using Orderbox.Repository.Voucher;
using Orderbox.RepositoryContract.Voucher;

namespace Orderbox.ServicesHook.DependencyInjection.Repositories
{
    public class VoucherRepositorySetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<ICustomerVoucherRepository, CustomerVoucherRepository>();
            service.AddScoped<IVoucherRepository, VoucherRepository>();
        }
    }
}
