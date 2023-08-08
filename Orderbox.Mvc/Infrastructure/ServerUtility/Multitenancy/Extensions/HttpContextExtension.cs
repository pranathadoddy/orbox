using Microsoft.AspNetCore.Http;
using Orderbox.Core;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy
{
    public static class HttpContextExtension
    {
        public static T GetTenant<T>(this HttpContext context) where T : Tenant
        {
            if (!context.Items.ContainsKey(CoreConstant.Tenant.HttpContextTenantKey))
                return null;
            return context.Items[CoreConstant.Tenant.HttpContextTenantKey] as T;
        }

        public static Tenant GetTenant(this HttpContext context)
        {
            return context.GetTenant<Tenant>();
        }
    }
}
