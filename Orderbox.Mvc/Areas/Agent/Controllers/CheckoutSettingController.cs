using Framework.Application.Controllers;
using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.Agent.Models;
using Orderbox.Mvc.Areas.Agent.Models.CheckoutSetting;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.ServiceContract.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Route("Agent/CheckoutSetting")]
    [Authorize(Roles = "Agent")]
    [IsTenantAccessibleByAgent]
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

        [HttpGet("Index/{tenantId}")]
        public IActionResult Index(ulong tenantId)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return NotFound();
            }

            var tenantDto = tenant as TenantDto;

            var model = new IndexModel
            {
                TenantId = tenantId,
                MerchantName = tenantDto.Name,
                SideNavigation = new SideNavigationModel
                {
                    MerchantId = tenantId,
                    ActivePage = "CheckoutSetting"
                },
                CustomerMustLogin = tenantDto.CustomerMustLogin,
                CheckoutForm = tenantDto.CheckoutForm,
                CheckoutForms = this.PopulateCheckoutForms(),
                GoogleOAuthClientId = tenantDto.GoogleOauthClientId,
                PaymentDtos = tenantDto.PaymentDtos
            };

            return View(model);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(IndexModel model)
        {
            object tenant;

            if (!this.HttpContext.Items.TryGetValue("tenant", out tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var tenantDto = tenant as TenantDto;

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