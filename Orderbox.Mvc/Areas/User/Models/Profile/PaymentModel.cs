using Microsoft.AspNetCore.Mvc.Rendering;
using Orderbox.Core.Resources.Account;
using Orderbox.Core.Resources.Common;
using Orderbox.Dto.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Security.Policy;

namespace Orderbox.Mvc.Areas.User.Models.Profile
{
    public class PaymentModel
    {
        #region Properties

        public ulong Id { get; set; }

        public string PaymentOptionCode { get; set; }

        public string ProviderNameBank { get; set; }

        public string ProviderNameWallet { get; set; }

        public string ProviderNamePaymentGateway { get; set; }

        public string PaymentGatewayProviderApiKey { get; set; }

        public string PaymentGatewayProviderWebHookSecret { get; set; }

        public int? PaymentGatewayMinuteDuration { get; set; }

        public string AccountName { get; set; }

        public string AccountNumberBank { get; set; }

        public string AccountNumberWallet { get; set; }


        #endregion
    }
}
