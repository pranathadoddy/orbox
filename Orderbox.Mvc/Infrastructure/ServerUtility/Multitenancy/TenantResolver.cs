using Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy.Store;
using Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy.Strategy;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy
{
    public class TenantResolver<T> where T : Tenant
    {
        private readonly ITenantResolutionStrategy _tenantResolutionStrategy;
        private readonly ITenantStore<T> _tenantStore;

        public TenantResolver(
            ITenantResolutionStrategy tenantResolutionStrategy,
            ITenantStore<T> tenantStore
        )
        {
            this._tenantResolutionStrategy = tenantResolutionStrategy;
            this._tenantStore = tenantStore;
        }

        public async Task<T> GetTenantAsync()
        {
            var tenantIdentifier = await _tenantResolutionStrategy.GetTenantIdentifierAsync();
            return await _tenantStore.GetTenantAsync(tenantIdentifier);
        }
    }
}
