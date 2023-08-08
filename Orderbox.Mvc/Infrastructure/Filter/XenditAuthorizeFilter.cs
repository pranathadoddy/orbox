using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Orderbox.Core.Resources.Common;
using Orderbox.Mvc.Models.PaymentGateway;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Transaction;
using System;
using System.IO;
using System.Linq;

namespace Orderbox.Mvc.Infrastructure.Filter
{
    public class XenditAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public XenditAuthorizeFilter(
            IOrderService orderService,
            IPaymentService paymentService)
        {
            this._orderService = orderService;
            this._paymentService = paymentService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headers = context.HttpContext.Request.Headers;
            var hasToken = headers.TryGetValue("X-Callback-Token", out StringValues token);

            if (!hasToken)
            {
                this.SetupErrorResult(context, GeneralResource.Unauthorized);
                return;
            }

            using StreamReader stream = new StreamReader(context.HttpContext.Request.Body);

            try
            {
                var body = stream.ReadToEndAsync().GetAwaiter().GetResult();
                var model = System.Text.Json.JsonSerializer.Deserialize<XenditCallbackPayloadModel>(body);

                var orderId = ulong.Parse(model.ExternalId);

                var orderResponse = 
                    this._orderService
                        .ReadAsync(new GenericRequest<ulong>
                        {
                            Data = orderId
                        })
                        .GetAwaiter()
                        .GetResult();

                if (orderResponse.IsError())
                {
                    this.SetupErrorResult(context, orderResponse.GetErrorMessage());
                    return;
                }

                var orderDto = orderResponse.Data;

                var paymentResponse =
                    this._paymentService
                        .PagedSearchAsync(new PagedSearchRequest
                        {
                            PageIndex = 0,
                            PageSize = 1,
                            OrderByFieldName = "id",
                            SortOrder = "asc",
                            Keyword = string.Empty,
                            Filters = $"tenantId={orderDto.TenantId} and paymentOptionCode=\"PMGW\" and providerName=\"XENDIT\""
                        })
                        .GetAwaiter()
                        .GetResult();

                if (paymentResponse.TotalCount == 0)
                {
                    this.SetupErrorResult(context, PaymentResource.NoPaymentGwSetupFound);
                    return;
                }

                var paymentDto = paymentResponse.DtoCollection.First();

                if (paymentDto.WebhookValidationSecret != token.ToString())
                {
                    this.SetupErrorResult(context, GeneralResource.Unauthorized);
                    return;
                }

                context.HttpContext.Items.Add("xenditCbModel", model);
                context.HttpContext.Items.Add("orderDto", orderDto);
            }
            catch(Exception e)
            {
                this.SetupErrorResult(context, e.Message);
            }
        }

        private void SetupErrorResult(AuthorizationFilterContext context, string message)
        {
            var content = JsonConvert.SerializeObject(
                new
                {
                    IsSuccess = false,
                    MessageErrorTextArray = new string[] { message }
                },
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            );
            context.Result = new JsonResult(content);
        }
    }
}
