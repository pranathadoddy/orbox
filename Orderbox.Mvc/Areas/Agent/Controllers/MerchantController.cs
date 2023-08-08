using Framework.Application.Controllers;
using Framework.Application.ModelBinder;
using Framework.Application.Models;
using Framework.Core;
using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Core;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.Agent.Models;
using Orderbox.Mvc.Areas.Agent.Models.Merchant;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Authorize(Roles = "Agent")]
    public class MerchantController : BaseController
    {
        private readonly ITenantService _tenantService;

        public MerchantController(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            ITenantService tenantService) : 
            base(configuration, webHostEnvironment)
        {
            this._tenantService = tenantService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var agencyId = this.User.Identity.GetAgencyId();
            var stringAgencyId = Cryptographer.Base64OTPEncrypt(agencyId);
            var urlFormat = this.Configuration.GetValue<string>("Application:UrlFormat");
            var rootDomain = this.Configuration.GetValue<string>("Application:RootDomain");
            var path = $"/Account/Registration?aid={stringAgencyId}";
            var url = $"{string.Format(urlFormat, rootDomain)}{path}";
            var model = new IndexModel
            {
                MerchantRegistrationUrl = url
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> PagedSearchGridJson([ModelBinder(typeof(GridModelBinder))] GridModel model)
        {
            var agencyId = this.User.Identity.GetAgencyId();

            var filters = model.Filters + (string.IsNullOrEmpty(model.Filters) ? "" : " and ") + $"AgencyId = {agencyId}";
            var response = await this._tenantService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = model.PageIndex - 1,
                PageSize = model.PageSize,
                OrderByFieldName = "Id",
                SortOrder = CoreConstant.SortOrder.Descending,
                Keyword = model.Keyword,
                Filters = filters
            });

            var rowJsonData = new List<object>();
            foreach (var dto in response.DtoCollection)  
            {
                rowJsonData.Add(new
                {
                    dto.Id,
                    dto.Name,
                    dto.EnableShop
                });
            }

            return GetPagedSearchGridJson(model.PageIndex, model.PageSize, rowJsonData, response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ulong id)
        {
            var agencyId = this.User.Identity.GetAgencyId();

            var response = await this._tenantService.PagedSearchAsync(new PagedSearchRequest {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"agencyId={agencyId} and id={id}"
            });

            if (response.TotalCount == 0)
            {
                return NotFound();
            }

            var tenantDto = response.DtoCollection.First();

            var model = new EditModel
            {
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = id,
                    ActivePage = "Edit"
                },
                Id = tenantDto.Id,
                Name = tenantDto.Name,
                Url = "",
                ShopEnabled = tenantDto.EnableShop,
                AllowToAccessProduct = tenantDto.AllowToAccessProduct,
                AllowToAccessCategory = tenantDto.AllowToAccessCategory,
                AllowToAccessProfile = tenantDto.AllowToAccessProfile,
                AllowToAccessCheckoutSetting = tenantDto.AllowToAccessCheckoutSetting
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditModel model)
        {
            var readResponse = await this._tenantService.PagedSearchAsync(new PagedSearchRequest
            {
                PageIndex = 0,
                PageSize = 1,
                OrderByFieldName = "Id",
                SortOrder = "asc",
                Keyword = string.Empty,
                Filters = $"Id={model.Id}"
            });

            if (!readResponse.DtoCollection.Any())
            {
                return this.GetErrorJson(GeneralResource.Item_NotFound);
            }

            var dto = readResponse.DtoCollection.First();

            dto.EnableShop = model.ShopEnabled;
            dto.AllowToAccessCategory = model.AllowToAccessCategory;
            dto.AllowToAccessProduct = model.AllowToAccessProduct;
            dto.AllowToAccessProfile = model.AllowToAccessProfile;
            dto.AllowToAccessCheckoutSetting = model.AllowToAccessCheckoutSetting;

            this.PopulateAuditFieldsOnUpdate(dto);

            var editResponse = await this._tenantService.UpdateAsync(new GenericRequest<TenantDto>
            {
                Data = dto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }
    }
}