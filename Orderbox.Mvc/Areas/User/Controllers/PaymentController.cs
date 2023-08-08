using Framework.Application.Controllers;
using Framework.ServiceContract.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Orderbox.Core.Resources.Common;
using Orderbox.Core.SystemCode;
using Orderbox.Dto.Common;
using Orderbox.Mvc.Areas.User.Models.Profile;
using Orderbox.Mvc.Infrastructure.Attributes;
using Orderbox.Mvc.Infrastructure.ServerUtility.Identity;
using Orderbox.ServiceContract.Common;
using System;
using System.Threading.Tasks;

namespace Orderbox.Mvc.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    [Tenant]
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

        [HttpPost]
        public async Task<IActionResult> Create(PaymentModel model)
        {
            var tenantId = this.User.Identity.GetTenantId();

            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            if (!IsValidPayment(model))
            {
                return this.GetErrorJson(PaymentResource.InvalidPayment);
            }

            var dto = new PaymentDto
            {
                TenantId = Convert.ToUInt64(tenantId),
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

        [HttpPost]
        public async Task<IActionResult> Delete(ulong Id)
        {
            if (!ModelState.IsValid)
            {
                return this.GetErrorJsonFromModelState();
            }

            var response = await this._PaymentService.DeleteAsync(new GenericRequest<ulong>
            {
                Data = Id
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