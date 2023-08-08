using Framework.Application.Controllers;
using Framework.Core;
using Framework.ServiceContract;
using Framework.ServiceContract.FileUpload.Request;
using Framework.ServiceContract.FileUpload.Response;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Core;
using Orderbox.Core.Resources.Common;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Dto.Transaction;
using Orderbox.Mvc.Areas.Tenant.Models.Order;
using Orderbox.Mvc.Infrastructure.ServerUtility.Multitenancy;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using Orderbox.ServiceContract.Payment;
using Orderbox.ServiceContract.Payment.Request;
using Orderbox.ServiceContract.PushNotification;
using Orderbox.ServiceContract.PushNotification.Request;
using Orderbox.ServiceContract.Transaction;
using Orderbox.ServiceContract.Utility;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Orderbox.Mvc.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentGatewayManager _paymentGatewayManager;
        private readonly IPaymentProofAssetsManager _paymentProofAssetsManager;
        private readonly IProductImageAssetsManager _productImageAssetsManager;
        private readonly IProductService _productService;
        private readonly IProductStoreService _productStoreService;
        private readonly IPushNotificationManager _pushNotificationManager;
        private readonly IRecaptchaValidator _recaptchaValidator;
        private readonly ITenantLogoAssetsManager _tenantLogoAssetsManager;
        private readonly ITenantService _tenantService;

        public OrderController(
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment,
            IOrderService orderService,
            IPaymentService paymentService,
            IPaymentGatewayManager paymentGatewayManager,
            IPaymentProofAssetsManager paymentProofAssetsManager,
            IProductImageAssetsManager productImageAssetsManager,
            IProductService productService,
            IProductStoreService productStoreService,
            IPushNotificationManager pushNotificationManager,
            IRecaptchaValidator recaptchaValidator,
            ITenantLogoAssetsManager tenantLogoAssetsManager,
            ITenantService tenantService
        ) : base(configuration, hostEnvironment)
        {
            this._orderService = orderService;
            this._paymentService = paymentService;
            this._paymentGatewayManager = paymentGatewayManager;
            this._paymentProofAssetsManager = paymentProofAssetsManager;
            this._productImageAssetsManager = productImageAssetsManager;
            this._productService = productService;
            this._productStoreService = productStoreService;
            this._pushNotificationManager = pushNotificationManager;
            this._recaptchaValidator = recaptchaValidator;
            this._tenantLogoAssetsManager = tenantLogoAssetsManager;
            this._tenantService = tenantService;
        }

        [HttpPost("Tenant/Order/SubmitOrder")]
        public async Task<ActionResult> SubmitOrderAsync([FromBody] CreateModel model)
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

        [HttpGet("Tenant/Order/Get/{id}")]
        public async Task<ActionResult> GetAsync(string id)
        {
            var tenant = HttpContext.GetTenant();

            var plainOrderId = Cryptographer.Base64OTPDecrypt(id);
            var stringOrderId = plainOrderId.Replace(CoreConstant.Settings.OrderboxIdPrefix, "");

            ulong orderId;
            if (!ulong.TryParse(stringOrderId, out orderId))
            {
                return NotFound();
            }

            var tenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong> { Data = tenant.Id });
            var tenantDto = tenantResponse.Data;

            var orderResponse = await this._orderService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenant.Id,
                Data = orderId
            });
            var orderDto = orderResponse.Data;

            var defaultTenantLogo = "data:image/png;base64,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";
            var defaultProductImage = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAOEAAADhCAMAAAAJbSJIAAAAMFBMVEXh6vH////5+vrl7fPv8/f0+fzt9Pbf6vD+/f/j6/Lf6vL///3f6fLr8fbv8/by9vm0HxD7AAACbUlEQVR4nO3c65abIBRAYVHTDALx/d+2VBuDII6zLHro2t/fmEn2HETn2jQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADkG29irNHa4I9G9+qu83B2BTauu01JIIYUUUkghhbcX/ipFTGH3LKOTUmi6Qq/QGQrLovA8Ckuj8LzqC/98A2ZX5YXd2Lajf6beOaTmwm5+tnnsBNZc+NTt/OaNanWT/U5hzYWjWjzyh1VSqK1ONpTPW/fyz66kcHCqixJtHwSqPvvUKgq1D/Rjik61MZzhWPd5aJ3fTfwR6yn+P4XzBFU8RevCwvzPJCoo9IFmepdmPUUdFuqaZzgt0fcUw8T+88Ar/wrSC/UywemgcKE+7Tg/ZNRY8V2bDScYT9G6r+mdu+fOK8guXE8wmaK/Vet6980PPmUXDvEEVXLRCL942lwGkguny0RSmF76//ILuh/SE1JyYXIObk/xzfn17NJH5BZ+LvSHpmjno10yRbmFySYTDHFjivONnZ9iHC+3MLNE31NcJy7zNsn9m9DC9DIRTXG9UD977jTF1UIVWpjbZDJTDM/YeLsRWfjdBKMprj8d8RRFFm5d6Dca31OM99z1diOwMHehT01TTBe0WV00BBYem+AyRbdxcDhFgYX6YN80RZu5LTCSZ9gcLzSqz+xIZvloAgt/MsN8u+QZUkghhRRSSCGFFFJIIYWXFKqv85Towr3fcDpKSy78xygsjcLzKCyNwvPEFCpthxKsVlIKXVeGE1NYHoUUUkghhRRSeFfh2D6u0o53BOpmuOz/RA17f8MHAAAAAAAAAAAAAAAAAAAAAAAAAAAAALjcb3yLQG5tF3tgAAAAAElFTkSuQmCC";

            this._tenantLogoAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });
            this._productImageAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });
            this._paymentProofAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });

            var tenantLogoUrlResponse = this._tenantLogoAssetsManager.GetUrl(new GenericRequest<string> { Data = tenantDto.Logo });
            var paymentProofUrlResponse = this._paymentProofAssetsManager.GetUrl(new GenericRequest<string> { Data = orderDto.PaymentProof });

            var model = new OrderDetailModel
            {
                OrderId = orderId,
                TenantName = tenantDto.Name,
                TenantLogoUrl = tenantLogoUrlResponse.Data ?? defaultTenantLogo,
                TenantPhoneAreaCode = tenantDto.PhoneAreaCode,
                TenantPhone = tenantDto.Phone,
                TenantUnderAgency = tenantDto.AgencyId.HasValue,
                Currency = tenantDto.Currency,
                OrderNumber = orderDto.OrderNumber,
                BuyerName = orderDto.BuyerName,
                OrderStatus = OrderStatusCode.Item.GetDescription(orderDto.Status),
                PaymentOptionCode = orderDto.PaymentOptionCode,
                PaymentProviderName = orderDto.PaymentProviderName,
                PaymentAccountName = orderDto.PaymentAccountName,
                PaymentAccountNumber = orderDto.PaymentAccountNumber,
                PaymentDescription = orderDto.PaymentDescription,
                PaymentStatus = orderDto.PaymentStatus,
                PaymentGatewayInvoiceUrl = orderDto.PaymentGatewayInvoiceUrl,
                PaymentProof = paymentProofUrlResponse.Data,
                OrderItems = orderDto.OrderItems.Select(item => new OrderItemModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductImageUrl =
                        string.IsNullOrEmpty(item.ProductImage) ?
                            defaultProductImage :
                            this._productImageAssetsManager.GetUrl(new GenericRequest<string> { Data = item.ProductImage }).Data,
                    Unit = item.ProductUnit,
                    Discount = item.Discount,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Note = item.Note ?? ""
                }).ToList()
            };

            return View("OrderDetail", model);
        }

        [HttpGet("Tenant/Order/Invoice/{id}")]
        public async Task<ActionResult> InvoiceAsync(string id)
        {
            ulong tenantId = 0;
            ulong orderId = 0;

            var validateInvoice = ValidateInvoice(id, ref tenantId, ref orderId);

            if (!validateInvoice)
            {
                Redirect("Home/Error?code=404");
            }

            var response = await this._orderService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenantId,
                Data = orderId
            });

            var dto = response.Data;

            var model = new InvoiceModel
            {
                Order = dto
            };

            return View("Invoice", model);
        }

        [HttpPost("Tenant/Order/UploadPaymentProof")]
        public async Task<ActionResult> UploadPaymentProof(UploadFileModel model)
        {
            if(!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var orderResponse = await this._orderService.ReadAsync(new GenericRequest<ulong>
            {
                Data = model.OrderId
            });

            if (orderResponse.IsError())
            {
                return this.GetErrorJson(orderResponse);
            }

            var orderDto = orderResponse.Data;

            var tenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>
            {
                Data = orderResponse.Data.TenantId
            });

            if (tenantResponse.IsError())
            {
                return this.GetErrorJson(tenantResponse);
            }

            var tenantDto = tenantResponse.Data;

            var uploadResponse = await this.UploadImageBase64(model.Base64File, model.FileName, tenantDto.ShortName, this._paymentProofAssetsManager);

            if (uploadResponse.IsError())
            {
                return this.GetErrorJson(uploadResponse);
            }

            orderDto.PaymentProof = uploadResponse.ServerFileName;
            this.PopulateAuditFieldsOnUpdate(orderDto);

            var editResponse = await this._orderService.UpdateAsync(new GenericRequest<OrderDto>
            {
                Data = orderDto
            });

            if (editResponse.IsError()) 
            {
                return this.GetErrorJson(editResponse);
            }

            return GetSuccessJson(editResponse, null);
        }

        private async Task<OrderDto> PopulateOrderDtoAsync(PaymentDto paymentDto, CreateModel model)
        {
            var tenant = HttpContext.GetTenant();

            var orderDto = new OrderDto()
            {
                BuyerName = model.BuyerName,
                BuyerPhoneNumber = model.BuyerPhone,
                BuyerEmailAddress = model.BuyerEmail,
                Description = model.Description,
                Date = DateTime.Now,
                PaymentStatus = CoreConstant.PaymentStatus.Ready,
                Status = CoreConstant.OrderStatus.New,
                TenantId = tenant.Id,
                OrderNumber = await this.GenerateOrderNumberAsync()
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

            var productIds = model.OrderItems.Select(item => $"Id={item.ProductId}").ToArray();
            var productFilters = string.Join(" or ", productIds);
            var productResponse = await this._productService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = model.OrderItems.Count,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"TenantId={tenant.Id} and ({productFilters})"
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
                Filters = $"TenantId={tenant.Id} and ({productStoreFilters})"
            });

            foreach (var orderItem in model.OrderItems)
            {
                var product = productResponse.DtoCollection.First(item => item.Id == orderItem.ProductId);
                var productStores = 
                    productStoreResponse
                        .DtoCollection
                        .Where(item => item.ProductId == orderItem.ProductId)
                        .Select(ps => new {
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
                    ProductRedeemMethod = product.RedeemMethod,
                    ProductDescription = product.Description,
                    ProductTermAndCondition = product.TermAndCondition,
                    ProductType = product.Type,
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

        private async Task<string> GenerateOrderNumberAsync()
        {
            var tenant = HttpContext.GetTenant();

            var latestOrderResponse = await this._orderService.GetLatestOrderOfTheCurrentTenantAsync(new GenericRequest<ulong> { Data = tenant.Id });
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
            var newOrderNumber = $"#{CoreConstant.Settings.OrderboxIdPrefix}{tenant.Id}{formatedDate}{customOrderNumber}";

            return newOrderNumber;
        }

        private async Task SendNotificationAndRetry3TimesIfUnsuccessfulAsync(OrderDto orderDto)
        {
            var tenant = HttpContext.GetTenant();
            var tenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong> { Data = tenant.Id });
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

        private bool ValidateInvoice(string id, ref ulong tenantId, ref ulong orderId)
        {
            var splitString = id.Split("o");

            if (splitString.Length != 2)
            {
                return false;
            }

            var stringTenantId = splitString[0].Remove(0, 1);
            var stringOrderId = splitString[1];

            if (!ulong.TryParse(stringTenantId, out tenantId) || !ulong.TryParse(stringOrderId, out orderId))
            {
                return false;
            }

            return true;
        }

        private async Task<FileUploadResponse> UploadImageBase64(string base64PngImage, string fileName, string tenantShortName, IAssetsManagerBase assetsManager)
        {
            var trimmedBase64Image = base64PngImage.Replace("data:image/png;base64,", "");
            var image = Convert.FromBase64String(trimmedBase64Image);
            using (var memoryStream = new MemoryStream(image))
            {
                assetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantShortName });
                var uploadResponse = await assetsManager.UploadAsync(new FileUploadRequest
                {
                    FileStream = memoryStream,
                    FileName = fileName,
                    FileSize = (ulong)memoryStream.Length,
                    MimeType = "image/png"
                });

                return uploadResponse;
            }
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
