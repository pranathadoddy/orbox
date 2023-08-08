using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Voucher;
using Orderbox.ServiceContract.Voucher;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class VoucherServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<ICustomerVoucherService, CustomerVoucherService>();
            service.AddScoped<IVoucherService, VoucherService>();
        }
    }
}
