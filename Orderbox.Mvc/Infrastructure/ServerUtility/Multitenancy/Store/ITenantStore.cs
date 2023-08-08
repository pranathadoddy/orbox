using System.Threading.Tasks;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy.Store
{
    public interface ITenantStore<T> where T : Tenant
    {
        Task<T> GetTenantAsync(string identifier);
    }
}
