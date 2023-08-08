using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Application.Presentation;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Dto.Transaction;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.Mvc.Models.Order;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Transaction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Authorize(Roles = "User")]
    [Tenant]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ITenantService _tenantService;

        public OrderController(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            IOrderService orderService,
            ITenantService tenantService) : 
            base(configuration, webHostEnvironment)
        {
            this._orderService = orderService;
            this._tenantService = tenantService;
        }

        #region Public Methods

        [HttpGet]
        public IActionResult Index(string status = "")
        {
            return View("Index",status);
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var tenantId = this.User.Identity.GetTenantId();

            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? "" : " and ") + $"tenantId = {tenantId}";

            var response = await this._orderService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = string.IsNullOrEmpty(model.OrderByFieldName) ? "LastModifiedDateTime" : model.OrderByFieldName,
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

        [HttpGet]
        public async Task<IActionResult> View(ulong id)
        {
            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var tenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong> { Data = tenantId });
            var tenantDto = tenantResponse.Data;

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

            var model = new ViewModel
            {
                Order = dto,
                Status = dto.Status,
                StatusRadioItems = this.GetOrderStatusRadioItems(),
                Currency = tenantDto.Currency
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(ViewModel model) 
        {
            var stringTenantId = this.User.Identity.GetTenantId();
            var tenantId = ulong.Parse(stringTenantId);

            var readResponse = await this._orderService.TenantReadAsync(new GenericTenantRequest<ulong>
            {
                TenantId = tenantId,
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

        #endregion

        #region Populate Grid Data

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

        private IEnumerable<RadioItem> GetOrderStatusRadioItems()
        {
            var radioItems = new List<RadioItem>();

            foreach(var status in OrderStatusCode.Item.ToDictionary())
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

        #endregion
    }
}