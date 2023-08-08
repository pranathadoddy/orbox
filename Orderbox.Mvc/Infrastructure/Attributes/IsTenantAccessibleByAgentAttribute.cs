using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Orderbox.Core.Resources.Common;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Infrastructure.Attributes
{
    public class IsTenantAccessibleByAgentAttribute : ActionFilterAttribute
    {
        private ITenantService _tenantService;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            this._tenantService = (ITenantService)context.HttpContext.RequestServices.GetService(typeof(ITenantService));
            Dictionary<string, object> queryCollection = null;

            if (context.HttpContext.Request.Method == "POST")
            {
                if (context.HttpContext.Request.ContentType != null && context.HttpContext.Request.ContentType.Contains("application/json"))
                {
                    var model = context.ActionArguments.FirstOrDefault(item => item.Key == "model");
                    var jsonModel = JsonConvert.SerializeObject(model.Value);
                    queryCollection = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonModel);
                }
                else
                {
                    try
                    {
                        queryCollection = context.HttpContext.Request.Form.ToDictionary(f => f.Key, f => (object)f.Value);
                    }
                    catch {} 
                }
            }
            else
            {
                queryCollection = context.HttpContext.Request.Query.ToDictionary(q => q.Key, q => (object)q.Value);
            }

            var agencyId = context.HttpContext.User.Identity.GetAgencyId();
            var tenantId = queryCollection?.FirstOrDefault(item => item.Key.ToLower() == "tenantid").Value;
            var filters = queryCollection?.FirstOrDefault(item => item.Key.ToLower() == "filters").Value;

            if (tenantId == null && filters != null && filters.ToString().Contains("tenantId"))
            {
                var filterSplitted = filters.ToString().Split(" ");
                var tenantFilter = filterSplitted.FirstOrDefault(item => item.Contains("tenantId"));
                var tenantFilterSplitted = tenantFilter.Split("=");
                tenantId = tenantFilterSplitted[1];
            }

            if (tenantId == null && context.HttpContext.Request.RouteValues.ContainsKey("tenantId"))
            {
                tenantId = context.HttpContext.Request.RouteValues["tenantId"];
            }

            if (tenantId == null)
            {
                if (context.HttpContext.Request.Method == "POST")
                {
                    context.Result = new JsonResult(new { IsSuccess = false, MessageErrorTextArray = new string[] { TenantResource.TenantIdIsRequired } });
                }
                else
                {
                    context.Result = new BadRequestResult();
                }                
                return;
            }

            var readTenantResponse = await this._tenantService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"id={tenantId} and agencyId={agencyId}"
            });

            if (readTenantResponse.IsError() || !readTenantResponse.DtoCollection.Any())
            {
                if (context.HttpContext.Request.Method == "POST")
                {
                    context.Result = new JsonResult(new { IsSuccess = false, MessageErrorTextArray = new string[] { GeneralResource.General_Forbidden } });
                }
                else
                {
                    context.Result = new ForbidResult();
                }
                return;
            }

            var tenantDto = readTenantResponse.DtoCollection.First();

            context.HttpContext.Items.Add(KeyValuePair.Create<object, object>("tenant", tenantDto));

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
