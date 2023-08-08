using Framework.Application.Controllers;
using Framework.Core.Resources;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Core.Resources.Common;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.Agent.Models.Payment;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.ServiceContract.Common;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Route("Agent/Payment")]
    [Authorize(Roles = "Agent")]
    [IsTenantAccessibleByAgent]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _PaymentService;

        public PaymentController(
            IConfiguration configuration,
            IPaymentService PaymentService,
            IWebHostEnvironment webHostEnvironment) :
            base(configuration, webHostEnvironment)
        {
            this._PaymentService = PaymentService;
        }

        #region Public Methods

        [HttpPost("Create")]
        public async Task<IActionResult> Create(PaymentModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            if (!IsValidPayment(model))
            {
                return this.GetErrorJson(PaymentResource.InvalidPayment);
            }

            var dto = new PaymentDto
            {
                TenantId = model.TenantId,
                PaymentOptionCode = model.PaymentOptionCode,
                ProviderName =
                    model.PaymentOptionCode == PaymentOptionCode.Item.CashOnDelivery.Value ? 
                        PaymentOptionCode.Item.CashOnDelivery.Description :
                            model.PaymentOptionCode == PaymentOptionCode.Item.Bank.Value ? 
                                BankCode.Item.GetDescription(model.ProviderNameBank) :
                                model.PaymentOptionCode == PaymentOptionCode.Item.Wallet.Value ?
                                    WalletCode.Item.GetDescription(model.ProviderNameWallet) :
                                        PaymentGatewayCode.Item.GetDescription(model.ProviderNamePaymentGateway),
                AccountNumber =
                    model.PaymentOptionCode == PaymentOptionCode.Item.CashOnDelivery.Value || model.PaymentOptionCode == PaymentOptionCode.Item.PaymentGateway.Value ? 
                        string.Empty :
                        model.PaymentOptionCode == PaymentOptionCode.Item.Bank.Value ? 
                            model.AccountNumberBank : 
                            model.AccountNumberWallet,
                AccountName =
                    model.PaymentOptionCode == PaymentOptionCode.Item.CashOnDelivery.Value || model.PaymentOptionCode == PaymentOptionCode.Item.PaymentGateway.Value ? 
                        string.Empty : model.AccountName,

                Description =
                    model.PaymentOptionCode == PaymentOptionCode.Item.CashOnDelivery.Value ?
                        PaymentResource.CODDescription : string.Empty,

                ApiKey =
                    model.PaymentOptionCode == PaymentOptionCode.Item.PaymentGateway.Value ?
                        model.PaymentGatewayProviderApiKey : string.Empty,

                WebhookValidationSecret =
                    model.PaymentOptionCode == PaymentOptionCode.Item.PaymentGateway.Value ?
                        model.PaymentGatewayProviderWebHookSecret : string.Empty,

                ActiveDurationInMinutes =
                    model.PaymentOptionCode == PaymentOptionCode.Item.PaymentGateway.Value ?
                        model.PaymentGatewayMinuteDuration : null,
            };

            this.PopulateAuditFieldsOnCreate(dto);

            var response = await this._PaymentService.InsertAsync(new GenericRequest<PaymentDto>
            {
                Data = dto
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, response.Data);
        }

        [HttpPost("Delete/{tenantId}/{id}")]
        public async Task<IActionResult> Delete(ulong tenantId, ulong id)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            if (!this.HttpContext.Items.TryGetValue("tenant", out var tenant))
            {
                return this.GetErrorJson(GeneralResource.General_AccessDenied);
            }

            var response = await this._PaymentService.DeleteAsync(new GenericRequest<ulong>
            {
                Data = id
            });

            if (response.IsError())
            {
                return this.GetErrorJson(response);
            }

            return this.GetSuccessJson(response, response.Data);
        }

        #endregion

        #region Private Method

        private bool IsValidPayment(PaymentModel model)
        {
            if (model.PaymentOptionCode != PaymentOptionCode.Item.CashOnDelivery.Value  
                && model.PaymentOptionCode != PaymentOptionCode.Item.PaymentGateway.Value
                && string.IsNullOrEmpty(model.AccountName))
                return false;

            if (model.PaymentOptionCode == PaymentOptionCode.Item.Bank.Value
                && string.IsNullOrEmpty(model.AccountNumberBank))
            {
                return false;
            }
            else if (model.PaymentOptionCode == PaymentOptionCode.Item.Wallet.Value
                && string.IsNullOrEmpty(model.AccountNumberWallet))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}