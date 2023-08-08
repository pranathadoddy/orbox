using Orderbox.Core.Resources.Account;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Models.Account
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "EmailAddress", ResourceType = typeof(LoginResource))]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(LoginResource))]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
