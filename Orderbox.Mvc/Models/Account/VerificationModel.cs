using Orderbox.Core.Resources.Account;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Models.Account
{
    public class VerificationModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [Display(Description = "VerificationCode", ResourceType = typeof(RegistrationResource))]
        public string Code { get; set; }
    }
}
