using Microsoft.Extensions.DependencyInjection;
using Orderbox.Service.Common;
using Orderbox.ServiceContract.Transaction;

namespace Orderbox.ServicesHook.DependencyInjection.Services
{
    public class TransactionServiceSetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IOrderService, OrderService>();
            service.AddScoped<IOrderItemService, OrderItemService>();
        }
    }
}