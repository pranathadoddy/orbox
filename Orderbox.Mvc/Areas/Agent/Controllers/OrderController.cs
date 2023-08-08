using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Application.Presentation;
using Framework.Core;
using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Dto.Transaction;
using Orderbox.Dto.Voucher;
using Orderbox.Mvc.Areas.Agent.Models;
using Orderbox.Mvc.Areas.Agent.Models.Order;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.FileUpload;
using Orderbox.ServiceContract.Transaction;
using Orderbox.ServiceContract.Voucher;
using Orderbox.ServiceContract.Voucher.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Route("Agent/Order")]
    [Authorize(Roles = "Agent")]
    [IsTenantAccessibleByAgent]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentProofAssetsManager _paymentProofAssetsManager;
        private readonly IOrderItemService _orderItemService;
        private readonly IVoucherService _voucherService;
        private readonly ICustomerVoucherService _customerVoucherService;

        public OrderController(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IOrderService orderService,
            IPaymentProofAssetsManager paymentProofAssetsManager,
            IOrderItemService orderItemService,
            IVoucherService voucherService,
            ICustomerVoucherService customerVoucherService) : 
            base(configuration, webHostEnvironment)
        {
            this._orderService = orderService;
            this._paymentProofAssetsManager = paymentProofAssetsManager;
            this._customerVoucherService = customerVoucherService;
            this._orderItemService = orderItemService;
            this._voucherService = voucherService;
        }

        [HttpGet("Index/{tenantId}/{status?}")]
        public IActionResult Index(ulong tenantId, string status)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            var model = new IndexModel
            {
                Status = string.IsNullOrEmpty(status) ? "" : status,
                MerchantName = tenantDto.Name,
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = tenantId,
                    ActivePage = "Order"
                }
            };

            return View(model);
        }

        [HttpPost("PagedSearchGridJson")]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var response = await this._orderService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = string.IsNullOrEmpty(model.OrderByFieldName) ? "LastModifiedDateTime" : model.OrderByFieldName,
                SortOrder = string.IsNullOrEmpty(model.SortOrder) ? "desc" : model.SortOrder,
                Keyword = model.Keyword,
                Filters = model.Filters
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)
            {
                rowJsonData.Add(this.PopulateResponse(dto));
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet("View/{tenantId}/{id}")]
        public async Task<IActionResult> View(ulong tenantId, ulong id)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            var response = await this._orderService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenantId,
                Data = id
            });

            if (response.IsError())
            {
                return NotFound();
            }

            var dto = response.Data;
            this._paymentProofAssetsManager.SetupSubDirectory(new GenericRequest<string> { Data = tenantDto.ShortName });
            var paymentProofResponse = this._paymentProofAssetsManager.GetUrl(new GenericRequest<string> { Data = dto.PaymentProof });

            var model = new ViewModel
            {
                Id = dto.Id,
                TenantId = tenantDto.Id,
                MerchantName = tenantDto.Name,
                TenantShortName = tenantDto.ShortName,
                Order = dto,
                PaymentProofUrl = paymentProofResponse.Data,
                Status = dto.Status,
                StatusRadioItems = this.GetOrderStatusRadioItems(),
                Currency = tenantDto.Currency,
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = tenantId,
                    ActivePage = "Order"
                }
            };

            return View(model);
        }

        [HttpPost("ResetPaymentProof/{id}")]
        public async Task<IActionResult> ResetPaymentProof(ulong id)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            var readResponse = await this._orderService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenantDto.Id,
                Data = id
            });

            if (readResponse.IsError())
            {
                return this.GetErrorJson(readResponse);
            }

            var dto = readResponse.Data;
            dto.PaymentProof = string.Empty;

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._orderService.UpdateAsync(new GenericRequest<OrderDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        [HttpPost("RejectPaymentProof/{id}")]
        public async Task<IActionResult> RejectPaymentProof(ulong id)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            var readResponse = await this._orderService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenantDto.Id,
                Data = id
            });

            if (readResponse.IsError())
            {
                return this.GetErrorJson(readResponse);
            }

            var dto = readResponse.Data;
            dto.PaymentStatus = CoreConstant.PaymentStatus.Cancelled;

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._orderService.UpdateAsync(new GenericRequest<OrderDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        [HttpPost("AcceptPaymentProof/{id}")]
        public async Task<IActionResult> AcceptPaymentProof(ulong id)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            var readResponse = await this._orderService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenantDto.Id,
                Data = id
            });

            if (readResponse.IsError())
            {
                return this.GetErrorJson(readResponse);
            }

            var dto = readResponse.Data;
            dto.PaymentStatus = CoreConstant.PaymentStatus.Paid;

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._orderService.UpdateAsync(new GenericRequest<OrderDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            var processOrderResponse = await ProcessOrderAsync(dto);
            if (processOrderResponse.IsError())
            {
                return this.GetErrorJson(processOrderResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(ViewModel model)
        {
            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return NotFound();
            }

            var readResponse = await this._orderService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = model.TenantId,
                Data = model.Id
            });

            if (readResponse.IsError())
            {
                return this.GetErrorJson(readResponse);
            }

            var dto = readResponse.Data;
            dto.Status = model.Status;

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._orderService.UpdateAsync(new GenericRequest<OrderDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        private object PopulateResponse(OrderDto dto)
        {
            return new
            {
                dto.Id,
                OrderNumber = string.IsNullOrEmpty(dto.OrderNumber) ? "Tidak Bernomor" : dto.OrderNumber,
                dto.BuyerName,
                dto.BuyerPhoneNumber,
                dto.Description,
                dto.Status,
                StatusDescription = OrderStatusCode.Item.GetDescription(dto.Status),
                StatusBadgeColor = this.GenerateBadgeColor(dto.Status)
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

        private IEnumerable<RadioItem> GetOrderStatusRadioItems()
        {
            var radioItems = new List<RadioItem>();

            foreach (var status in OrderStatusCode.Item.ToDictionary())
            {
                var item = new RadioItem()
                {
                    Value = status.Key,
                    Label = status.Value,
                };

                radioItems.Add(item);
            }

            return radioItems;
        }

        private async Task<BasicResponse> ProcessOrderAsync(OrderDto orderDto)
        {
            var response = new BasicResponse();

            var orderItemResponse = await this._orderItemService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1000,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"OrderId=\"{orderDto.Id}\""
            });

            if (orderItemResponse.TotalCount != 0)
            {
                var orderItems = orderItemResponse.DtoCollection;
                var voucherOrderItems = orderItems.Where(item => item.ProductType == CoreConstant.ProductType.Voucher);
                if (voucherOrderItems.Any())
                {
                    foreach (var voucherOrderItem in voucherOrderItems)
                    {
                        var vouchers = new List<VoucherDto>();

                        for (int i = 1; i <= voucherOrderItem.Quantity; i++)
                        {
                            CustomerVoucherDto customerVoucher = null;
                            if (orderDto.CustomerId.HasValue)
                            {
                                customerVoucher = new CustomerVoucherDto
                                {
                                    CustomerId = orderDto.CustomerId.Value
                                };
                                this.PopulateAuditFieldsOnCreate(customerVoucher);
                            }

                            var voucherCode =
                                    Cryptographer.NumberToLetter($"{orderDto.Id.ToString().PadLeft(2, '0')}-{voucherOrderItem.Id.ToString().PadLeft(2, '0')}-{i.ToString().PadLeft(2, '0')}");
                            var voucherDto = new VoucherDto
                            {
                                OrderItemId = voucherOrderItem.Id,
                                VoucherCode = voucherCode,
                                Name = voucherOrderItem.ProductName,
                                Description = voucherOrderItem.ProductDescription,
                                RedeemMethod = voucherOrderItem.ProductRedeemMethod,
                                Status = CoreConstant.VoucherStatus.Valid,
                                TermAndCondition = voucherOrderItem.ProductTermAndCondition,
                                ValidEndDate = voucherOrderItem.ValidEndDate.GetValueOrDefault(),
                                ValidStartDate = voucherOrderItem.ValidStartDate.GetValueOrDefault(),
                                CustomerVoucher = customerVoucher
                            };
                            this.PopulateAuditFieldsOnCreate(voucherDto);
                            vouchers.Add(voucherDto);
                        }

                        var insertResponse = await this._voucherService.BulkInsert(new GenericRequest<ICollection<VoucherDto>>
                        {
                            Data = vouchers
                        });

                        if (insertResponse.IsError())
                        {
                            response.AddErrorMessage(insertResponse.GetErrorMessage());
                        }
                    }
                }
            }

            return response;
        }
    }
}