using System.Threading.Tasks;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy.Strategy
{
    public interface ITenantResolutionStrategy
    {
        Task<string> GetTenantIdentifierAsync();
    }
}
