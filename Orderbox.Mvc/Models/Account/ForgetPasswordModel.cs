using Orderbox.Core.Resources.Account;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Models.Account
{
    public class ForgetPasswordModel
    {
        [Required]
        [Display(Name = "EmailAddress", ResourceType = typeof(LoginResource))]
        public string EmailAddress { get; set; }
    }
}
