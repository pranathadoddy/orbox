using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Core;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Api.Infrastructure.ServerUtility.Identity;
using Orderbox.Api.Models.Order;
using Orderbox.Core;
using Orderbox.Core.Resources.Common;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Dto.Transaction;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Payment;
using Orderbox.ServiceContract.Payment.Request;
using Orderbox.ServiceContract.PushNotification;
using Orderbox.ServiceContract.PushNotification.Request;
using Orderbox.ServiceContract.Transaction;
using Orderbox.ServiceContract.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Orderbox.Api.Controllers
{
    [Route("api/Order")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Google")]
    public class OrderController : ApiBaseController
    {
        private readonly ITenantService _tenantService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IRecaptchaValidator _recaptchaValidator;
        private readonly IPaymentGatewayManager _paymentGatewayManager;
        private readonly IPaymentService _paymentService;
        private readonly IProductService _productService;
        private readonly IProductStoreService _productStoreService;
        private readonly IPushNotificationManager _pushNotificationManager;

        public OrderController(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            ITenantService tenantService,
            IOrderService orderService,
            IRecaptchaValidator recaptchaValidator,
            ICustomerService customerService,
            IPaymentGatewayManager paymentGatewayManager,
            IPaymentService paymentService,
            IProductService productService,
            IProductStoreService productStoreService,
            IPushNotificationManager pushNotificationManager
        ) : base(configuration, hostEnvironment)
        {
            this._recaptchaValidator = recaptchaValidator;
            this._tenantService = tenantService;
            this._orderService = orderService;
            this._customerService = customerService;
            this._paymentGatewayManager = paymentGatewayManager;
            this._paymentService = paymentService;
            this._productService = productService;
            this._productStoreService = productStoreService;
            this._pushNotificationManager = pushNotificationManager;
        }

        [HttpGet]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var customerIdentityId = this.User.Identity.GetUserId();
            var customerResponse =
                await this._customerService.ReadByCustomerIdAsync(new GenericRequest<string>
                {
                    Data = customerIdentityId
                });

            if (customerResponse.IsError())
            {
                return GetPagedSearchGridJson(model.PageIndex, model.PageSize, new List<object>(), new GenericPagedSearchResponse<OrderDto>());
            }

            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? "" : " and ") + $"customerId = {customerResponse.Data.Id}";

            var response = await this._orderService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = string.IsNullOrEmpty(model.OrderByFieldName) ? "CreatedDateTime" : model.OrderByFieldName,
                SortOrder = string.IsNullOrEmpty(model.SortOrder) ? "desc" : model.SortOrder,
                Keyword = model.Keyword,
                Filters = filters
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var isValidCaptcha = await this._recaptchaValidator.IsValidResponseAsync(model.CaptchaToken);
            if (!isValidCaptcha)
            {
                return this.GetErrorJson(new string[] {
                    Core.Resources.Account.RegistrationResource.InvalidRegistration
                });
            }

            try
            {
                var paymentResponse = await this._paymentService.TenantReadAsync(new GenericTenantRequest<ulong>
                {
                    TenantId = model.TenantId,
                    Data = model.PaymentMethodId
                });

                var paymentDto = (PaymentDto)null;
                if (!paymentResponse.IsError())
                {
                    paymentDto = paymentResponse.Data;
                }

                var orderDto = await PopulateOrderDtoAsync(paymentDto, model);

                var response = await this._orderService.InsertAsync(new GenericRequest<OrderDto> { Data = orderDto });

                if (response.IsError())
                {
                    return this.GetErrorJson(response);
                }

                await this.SetupExternalInvoiceIfUsingPaymentGatewayAsync(response.Data.Id, paymentDto);

                await this.SendNotificationAndRetry3TimesIfUnsuccessfulAsync(orderDto);

                var orderNumber = $"{CoreConstant.Settings.OrderboxIdPrefix}{response.Data.Id}";
                var encryptedOrderId = Cryptographer.Base64OTPEncrypt(orderNumber);

                return this.GetSuccessJson(response, new { Token = HttpUtility.UrlEncode(encryptedOrderId), OrderNumber = orderDto.OrderNumber, OrderDateTime = orderDto.CreatedDateTime });
            }
            catch (Exception ex)
            {
                return this.GetErrorJson(ex.Message);
            }
        }

        private object PopulateResponse(OrderDto dto)
        {
            var orderNumber = $"{CoreConstant.Settings.OrderboxIdPrefix}{dto.Id}";
            var encryptedOrderId = Cryptographer.Base64OTPEncrypt(orderNumber);
            var domainPostfix = this.Configuration.GetValue<string>("Application:DomainPostfix");
            var urlFormat = this.Configuration.GetValue<string>("Application:UrlFormat");
            var baseUrl = string.Format(urlFormat, $"{dto.Tenant.ShortName}{domainPostfix}");
            var encodedOrderId = HttpUtility.UrlEncode(encryptedOrderId);
            return new
            {
                dto.Id,
                OrderNumber = string.IsNullOrEmpty(dto.OrderNumber) ? "Tidak Bernomor" : dto.OrderNumber,
                dto.BuyerName,
                dto.BuyerPhoneNumber,
                dto.Description,
                dto.Status,
                dto.PaymentStatus,
                dto.Date,
                Token = encodedOrderId,
                StatusDescription = OrderStatusCode.Item.GetDescription(dto.Status),
                StatusBadgeColor = this.GenerateBadgeColor(dto.Status),
                Url = $"{baseUrl}/order/get/{encodedOrderId}"
            };
        }
        private string GenerateBadgeColor(string status)
        {
            switch (status)
            {
                case CoreConstant.OrderStatus.New:
                    return "badge-orange";
                case CoreConstant.OrderStatus.InProgress:
                    return "badge-yellow";
                case CoreConstant.OrderStatus.Sent:
                    return "badge-green";
                case CoreConstant.OrderStatus.Cancelled:
                    return "badge-red";
            }

            return string.Empty;
        }

        private async Task SendNotificationAndRetry3TimesIfUnsuccessfulAsync(OrderDto orderDto)
        {
            var tenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong> { Data = orderDto.TenantId });
            var tenantDto = tenantResponse.Data;

            if (tenantDto.TenantPushNotificationTokenDto == null)
            {
                return;
            }

            var urlFormat = this.Configuration.GetValue<string>("Application:UrlFormat");
            var orderboxDomain = this.Configuration.GetValue<string>("Application:RootDomain");
            var tenantAdminUrl = string.Format(urlFormat, orderboxDomain);
            var orderAdminUrl = $"{tenantAdminUrl}/Order/View/{orderDto.Id}";

            var pushNotificationRetry = 0;
            var pushNotificationIsSuccess = false;
            do
            {
                var pushNotificationResponse = await this._pushNotificationManager.PushMessageAsync(new NotificationRequest
                {
                    Token = tenantDto.TenantPushNotificationTokenDto.Token,
                    Title = orderDto.BuyerName,
                    Body = string.Format(OrderResource.NewOrderNotificationBody, orderDto.OrderItems.Count()),
                    RedirectUrl = orderAdminUrl
                });

                pushNotificationRetry++;
                pushNotificationIsSuccess = pushNotificationResponse.Data;
            }
            while (!pushNotificationIsSuccess && pushNotificationRetry < 3);
        }


        private async Task<string> GenerateOrderNumberAsync(ulong tenantId)
        {
            var latestOrderResponse = await this._orderService.GetLatestOrderOfTheCurrentTenantAsync(new GenericRequest<ulong> { Data = tenantId });
            var latestOrderNumber = string.Empty;
            if (!latestOrderResponse.IsError())
            {
                latestOrderNumber = latestOrderResponse.Data.OrderNumber;
            }
            var currentDate = DateTime.UtcNow.Date;
            var customOrderNumber = string.Empty;

            if (string.IsNullOrEmpty(latestOrderNumber) || latestOrderResponse.Data.CreatedDateTime.Date != currentDate)
            {
                customOrderNumber = "1".PadLeft(4, '0');
            }
            else
            {
                var latestCustomOrderNumber = int.Parse(latestOrderNumber.Substring(latestOrderNumber.Length - 4));
                var newCustomOrderNumber = ++latestCustomOrderNumber;

                customOrderNumber = newCustomOrderNumber.ToString().PadLeft(4, '0');
            }

            var formatedDate = currentDate.ToString("yyyyMMdd");
            var newOrderNumber = $"#{CoreConstant.Settings.OrderboxIdPrefix}{tenantId}{formatedDate}{customOrderNumber}";

            return newOrderNumber;
        }

        private async Task<OrderDto> PopulateOrderDtoAsync(PaymentDto paymentDto, CreateModel model)
        {
            var buyerUserId = this.User.Identity.GetUserId();
            var authType = this.User.Identity.GetIssuer();

            var responseCustomer = await this._customerService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"authType=\"{authType}\" and externalId=\"{buyerUserId}\""
            });

            var customerDto = responseCustomer.DtoCollection.First();

            var orderDto = new OrderDto()
            {
                CustomerId = customerDto.Id,
                BuyerName = $"{customerDto.FirstName} {customerDto.LastName}",
                BuyerPhoneNumber = customerDto.Phone,
                BuyerEmailAddress = customerDto.EmailAddress,
                Description = model.Description,
                Date = DateTime.Now,
                PaymentStatus = CoreConstant.PaymentStatus.Ready,
                Status = CoreConstant.OrderStatus.New,
                TenantId = model.TenantId,
                OrderNumber = await this.GenerateOrderNumberAsync(model.TenantId)
            };
            this.PopulateAuditFieldsOnCreate(orderDto);

            if (paymentDto != null)
            {
                orderDto.PaymentOptionCode = paymentDto.PaymentOptionCode;
                orderDto.PaymentProviderName = paymentDto.ProviderName;
                orderDto.PaymentAccountName = paymentDto.AccountName;
                orderDto.PaymentAccountNumber = paymentDto.AccountNumber;
                orderDto.PaymentDescription = paymentDto.Description;
            }

            var orderItemIds = model.OrderItems.Select(item => $"Id={item.ProductId}").ToArray();
            var orderItemFilters = string.Join(" or ", orderItemIds);
            var productResponse = await this._productService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = model.OrderItems.Count,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = orderItemFilters
            });

            var productIdsForProductStoreFilter = model.OrderItems.Select(item => $"ProductId={item.ProductId}").ToArray();
            var productStoreFilters = string.Join(" or ", productIdsForProductStoreFilter);
            var productStoreResponse = await this._productStoreService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1000,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"TenantId={model.TenantId} and ({productStoreFilters})"
            });

            foreach (var orderItem in model.OrderItems)
            {
                var product = productResponse.DtoCollection.First(item => item.Id == orderItem.ProductId);
                var productStores =
                    productStoreResponse
                        .DtoCollection
                        .Where(item => item.ProductId == orderItem.ProductId)
                        .Select(ps => new
                        {
                            Id = ps.Store.Id,
                            Name = ps.Store.Name,
                            Address = ps.Store.Address,
                            MapUrl = ps.Store.MapUrl,
                            City = new
                            {
                                Id = ps.Store.City.Id,
                                Name = ps.Store.City.Name,
                                Country = new
                                {
                                    Id = ps.Store.City.Country.Id,
                                    Name = ps.Store.City.Country.Name
                                }
                            }
                        });
                var productStoresJson = JsonSerializer.Serialize(productStores);
                var orderItemDto = new OrderItemDto()
                {
                    TenantId = product.TenantId,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImage = product.ProductImages.FirstOrDefault(item => item.IsPrimary)?.FileName ?? "",
                    ProductUnit = product.Unit,
                    ProductType = product.Type,
                    ProductDescription = product.Description,
                    ProductRedeemMethod = product.RedeemMethod,
                    ProductTermAndCondition = product.TermAndCondition,
                    ValidStartDate = product.ValidPeriodStart,
                    ValidEndDate = product.ValidPeriodEnd,
                    Discount = product.Discount,
                    Commission = product.Commission,
                    UnitPrice = product.Price,
                    Quantity = orderItem.Quantity,
                    Note = orderItem.Note,
                    AvailableRedeemStores = productStoresJson
                };
                this.PopulateAuditFieldsOnCreate(orderItemDto);
                orderDto.OrderItems.Add(orderItemDto);
            }

            return orderDto;
        }

        private async Task SetupExternalInvoiceIfUsingPaymentGatewayAsync(ulong orderId, PaymentDto paymentDto)
        {
            if (paymentDto.PaymentOptionCode == PaymentOptionCode.Item.PaymentGateway.Value)
            {
                var orderResponse = await this._orderService.ReadAsync(new GenericRequest<ulong>
                {
                    Data = orderId
                });

                var orderDto = orderResponse.Data;

                var response = await this._paymentGatewayManager
                    .Handlers[paymentDto.ProviderName]
                    .CreatePurchaseAsync(new CreatePurchaseRequest
                    {
                        TenantId = orderDto.TenantId,
                        OrderId = orderDto.Id,
                        UserName = this.User.Identity.Name
                    });

                if (response.IsError())
                {
                    throw new Exception(response.GetErrorMessage());
                }

                orderDto.PaymentGatewayInvoiceUrl = response.Data;
                this.PopulateAuditFieldsOnUpdate(orderDto);

                var updateResponse = await this._orderService.UpdateAsync(new GenericRequest<OrderDto> { Data = orderDto });

                if (updateResponse.IsError())
                {
                    throw new Exception(updateResponse.GetErrorMessage());
                }
            }
        }
    }
}