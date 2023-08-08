using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Orderbox.Core.Resources.Common;
using Orderbox.Mvc.Areas.User.Controllers;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Infrastructure.Attributes
{
    public class TenantAttribute : ActionFilterAttribute
    {
        private ITenantService _tenantService;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            this._tenantService = (ITenantService)context.HttpContext.RequestServices.GetService(typeof(ITenantService));

            var stringTenantId = context.HttpContext.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var readTenantResponse = await this._tenantService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"id={tenantId}"
            });

            if (readTenantResponse.IsError() || !readTenantResponse.DtoCollection.Any())
            {
                context.Result = new ForbidResult();
                return;
            }

            var tenantDto = readTenantResponse.DtoCollection.First();
            
            if(!tenantDto.AllowToAccessCategory && context.Controller is CategoryController)
            {
                context.Result = new RedirectResult("/Order/Index");
                return;
            } 
            else if(!tenantDto.AllowToAccessCheckoutSetting && context.Controller is CheckoutSettingController)
            {
                context.Result = new RedirectResult("/Order/Index");
                return;
            }
            else if (!tenantDto.AllowToAccessProduct && context.Controller is ProductController)
            {
                context.Result = new RedirectResult("/Order/Index");
                return;
            }
            else if (!tenantDto.AllowToAccessProfile && context.Controller is ProfileController)
            {
                context.Result = new RedirectResult("/Order/Index");
                return;
            }
            context.HttpContext.Items.Add(KeyValuePair.Create<object, object>("tenant", tenantDto));

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
