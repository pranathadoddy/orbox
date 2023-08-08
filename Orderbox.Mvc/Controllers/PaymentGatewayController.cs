using Framework.Application.Controllers;
using Framework.Core;
using Framework.ServiceContract.Request;
using Framework.ServiceContract.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Core;
using Orderbox.Dto.Common;
using Orderbox.Dto.Transaction;
using Orderbox.Dto.Voucher;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Models.PaymentGateway;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Transaction;
using Orderbox.ServiceContract.Voucher;
using Orderbox.ServiceContract.Voucher.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Controllers
{
    public class PaymentGatewayController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IVoucherService _voucherService;
        private readonly ICustomerVoucherService _customerVoucherService;

        public PaymentGatewayController(
            IConfiguration configuration, 
            IHostEnvironment hostEnvironment,
            IOrderService orderService,
            IOrderItemService orderItemService,
            IVoucherService voucherService,
            ICustomerVoucherService _customerVoucherService
        ) : base(configuration, hostEnvironment)
        {
            this._orderService = orderService;
            this._orderItemService = orderItemService;
            this._voucherService = voucherService;
            this._customerVoucherService = _customerVoucherService;
        }

        [HttpPost]
        [XenditAuthorize]
        public async Task<IActionResult> XenditCallback()
        {
            object orderObject;
            object xenditCbObject;

            if (!this.HttpContext.Items.TryGetValue("orderDto", out orderObject))
            {
                return NotFound();
            }

            var orderDto = orderObject as OrderDto;

            if (!this.HttpContext.Items.TryGetValue("xenditCbModel", out xenditCbObject))
            {
                return NotFound();
            }

            var xenditCbModel = xenditCbObject as XenditCallbackPayloadModel;

            var xenditPaymentStatus = new Dictionary<string, string>
            {
                {"PAID", CoreConstant.PaymentStatus.Paid},
                {"EXPIRED", CoreConstant.PaymentStatus.Expired}
            };

            orderDto.PaymentChannel = xenditCbModel.PaymentChannel;
            orderDto.PaymentMethod = xenditCbModel.PaymentMethod;
            orderDto.PaidAt = xenditCbModel.PaidAt;
            orderDto.PaymentStatus = xenditPaymentStatus[xenditCbModel.Status];
            this.PopulateAuditFieldsOnUpdate(orderDto);

            var response = await this._orderService.UpdateAsync(new GenericRequest<OrderDto>
            {
                Data = orderDto
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            var processOrderResponse = await ProcessOrderAsync(orderDto);
            if (processOrderResponse.IsError())
            {
                return this.GetErrorJson(processOrderResponse);
            }


            return this.GetBasicSuccessJson();
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

                        var insertResponse = await this._voucherService.BulkInsert(new GenericRequest<ICollection<VoucherDto>> { 
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
