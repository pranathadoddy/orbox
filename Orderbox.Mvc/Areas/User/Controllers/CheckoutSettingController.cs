using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.User.Models.CheckoutSetting;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
    public class CheckoutSettingController : BaseController
    {
        private readonly ITenantService _tenantService;

        public CheckoutSettingController(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            ITenantService tenantService
        ) : base(configuration, hostEnvironment)
        {
            this._tenantService = tenantService;
        }

        #region Public Methods

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tenantId = ulong.Parse(this.User.Identity.GetTenantId());
            var readTenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>
            {
                Data = tenantId
            });

            var tenantDto = readTenantResponse.Data;

            var model = new IndexModel
            {
                CustomerMustLogin = tenantDto.CustomerMustLogin,
                CheckoutForm = tenantDto.CheckoutForm,
                CheckoutForms = this.PopulateCheckoutForms(),
                GoogleOAuthClientId = tenantDto.GoogleOauthClientId,
                PaymentDtos = tenantDto.PaymentDtos
            };

            return this.PartialView("_Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(IndexModel model)
        {
            var tenantId = ulong.Parse(this.User.Identity.GetTenantId());

            var readTenantResponse = await this._tenantService.ReadAsync(new GenericRequest<ulong>
            {
                Data = tenantId
            });

            if (readTenantResponse.IsError()) return this.GetErrorJson(readTenantResponse);

            var tenantDto = readTenantResponse.Data;

            tenantDto.CustomerMustLogin = model.CustomerMustLogin;
            tenantDto.GoogleOauthClientId = model.GoogleOAuthClientId;
            tenantDto.CheckoutForm = model.CheckoutForm;

            this.PopulateAuditFieldsOnUpdate(tenantDto);

            var editResponse = await this._tenantService.UpdateAsync(new GenericRequest<TenantDto>
            {
                Data = tenantDto
            });

            if (editResponse.IsError())
            {
                return this.GetErrorJson(editResponse);
            }

            return this.GetSuccessJson(editResponse, editResponse.Data);
        }

        #endregion

        private SelectList PopulateCheckoutForms()
        {
            var checkoutForms = CheckoutFormCode.Item.ToDictionary().ToList();
            checkoutForms.Insert(0, new KeyValuePair<string, string>("", ""));

            return new SelectList(checkoutForms, "Key", "Value");
        }
    }
}