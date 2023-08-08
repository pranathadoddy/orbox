using Microsoft.Extensions.DependencyInjection;
using Orderbox.Repository.Transaction;
using Orderbox.RepositoryContract.Transaction;

namespace Orderbox.ServicesHook.DependencyInjection.Repositories
{
    public class TransactionRepositorySetup
    {
        public static void Initialize(IServiceCollection service)
        {
            service.AddScoped<IOrderRepository, OrderRepository>();
            service.AddScoped<IOrderItemRepository, OrderItemRepository>();
        }
    }
}
