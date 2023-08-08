using Framework.RepositoryContract;
using Orderbox.Dto.Transaction;
using System.Threading.Tasks;

namespace Orderbox.RepositoryContract.Transaction
{
    public interface IOrderRepository : IBaseTenantRepository<OrderDto>
    {
        Task<OrderDto> GetLatestOrderOfTheCurrentTenantAsync(ulong tenantId);

        Task<OrderDto> ReadByIdAsync(ulong CustomerId, ulong Id);
    }
}
