using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Payment;
using Orderbox.ServiceContract.Payment;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class PaymentServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IPaymentGatewayManager, PaymentGatewayManager>();
            service.AddScoped<IXenditHandler, XenditHandler>();
        }
    }
}