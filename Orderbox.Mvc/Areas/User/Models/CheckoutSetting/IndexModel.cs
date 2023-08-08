using Framework.Application.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Orderbox.Core.Resources.Common;
using Orderbox.Dto.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.User.Models.CheckoutSetting
{
    public class IndexModel
    {
        #region Properties

        [Required]
        [Display(Name = "CustomerMustLogin", ResourceType = typeof(TenantResource))]
        public bool CustomerMustLogin { get; set; }

        [RequiredIfValidation("CustomerMustLogin", true)]
        [Required]
        [Display(Name = "GoogleOAuthClientId", ResourceType = typeof(TenantResource))]
        public string GoogleOAuthClientId { get; set; }

        [Required]
        [Display(Name = "CheckoutForm", ResourceType = typeof(TenantResource))]
        public string CheckoutForm { get; set; }

        public SelectList CheckoutForms { get; set; }

        public ICollection<PaymentDto> PaymentDtos { get; set; }

        #endregion
    }
}
