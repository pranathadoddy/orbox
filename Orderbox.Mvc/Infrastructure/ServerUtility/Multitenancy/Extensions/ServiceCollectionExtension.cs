using Microsoft.Extensions.DependencyInjection;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy
{
    public static class ServiceCollectionExtension
    {
        public static TenantBuilder<T> AddMultiTenancy<T>(this IServiceCollection services) where T : Tenant => new TenantBuilder<T>(services);

        public static TenantBuilder<Tenant> AddMultiTenancy(this IServiceCollection services) => new TenantBuilder<Tenant>(services);
    }
}
