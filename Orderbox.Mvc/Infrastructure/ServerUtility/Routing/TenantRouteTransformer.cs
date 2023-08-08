using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Orderbox.Core;
using Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Infrastructure.ServerUtility.Routing
{
    public class TenantRouteTransformer : DynamicRouteValueTransformer
    {
        public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            var tenantResolver = httpContext.RequestServices.GetService(typeof(TenantResolver<Tenant>)) as TenantResolver<Tenant>;
            var tenant = await tenantResolver.GetTenantAsync();

            if (tenant == null)
            {
                return values;
            };

            var controller = values["controller"] as string;
            var action = values["action"] as string;

            values.Add("area", "Tenant");
            if (string.IsNullOrEmpty(controller))
            {
                values["controller"] = "Home";
            }
            if (string.IsNullOrEmpty(action))
            {
                values["action"] = "Index";
            }
            httpContext.Items.Add(CoreConstant.Tenant.HttpContextTenantKey, tenant);

            return values;
        }
    }
}
