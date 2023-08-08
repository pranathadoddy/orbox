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
using Orderbox.Core;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Voucher;
using Orderbox.ServiceContract.Common;
using Orderbox.ServiceContract.Voucher;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orderbox.Api.Controllers
{
    [Route("api/Voucher")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Google")]
    public class VoucherController : ApiBaseController
    {
        private readonly ICustomerVoucherService _customerVoucherService;
        private readonly ICustomerService _customerService;

        public VoucherController(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            ICustomerVoucherService customerVoucherService,
            ICustomerService customerService
        ) : base(configuration, hostEnvironment)
        {
            this._customerVoucherService = customerVoucherService;
            this._customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var customerIdentityId = this.User.Identity.GetUserId();
            var customerResponse = await this._customerService.ReadByCustomerIdAsync(new GenericRequest<string>
            {
                Data = customerIdentityId
            });

            if (customerResponse.IsError())
            {
                return GetPagedSearchGridJson(model.PageIndex, model.PageSize, new List<object>(), new GenericPagedSearchResponse<CustomerVoucherDto>());
            }

            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? "" : " and ") + $"customerId = {customerResponse.Data.Id}";

            var response = await this._customerVoucherService.PagedSearchAsync(new PagedSearchRequest
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

        private object PopulateResponse(CustomerVoucherDto dto)
        {
            var voucherId = dto.VoucherId.ToString("D12");
            var encryptedVoucherId = Cryptographer.Base64OTPEncrypt(voucherId);
            var rootDomain = Configuration.GetValue<string>("Application:RootDomain");
            var urlFormat = Configuration.GetValue<string>("Application:UrlFormat");
            var url = string.Format($"{urlFormat}/voucher/{encryptedVoucherId}", rootDomain);

            return new
            {
                Id = dto.VoucherId,
                BadgeColor = GenerateBadgeColor(dto.Voucher.Status),
                Code = dto.Voucher.VoucherCode,
                Name = dto.Voucher.Name,
                Status = dto.Voucher.Status,
                StatusDescription = VoucherStatusCode.Item.GetDescription(dto.Voucher.Status),
                Url = url,
                CreatedDateTime = dto.Voucher.CreatedDateTime
            };
        }
        private string GenerateBadgeColor(string status)
        {
            switch (status)
            {
                case CoreConstant.VoucherStatus.Used:
                    return "badge-red";
                case CoreConstant.VoucherStatus.Valid:
                    return "badge-green";
            }

            return string.Empty;
        }
    }
}