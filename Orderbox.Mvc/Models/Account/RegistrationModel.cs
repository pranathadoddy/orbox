using Orderbox.Core.Resources.Account;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Models.Account
{
    public class RegistrationModel
    {
        [Required]
        [Display(Description = "EmailAddress", ResourceType = typeof(RegistrationResource))]
        public string EmailAddress { get; set; }

        public string CaptchaToken { get; set; }

        public string AgencyId { get; set; }
    }
}
