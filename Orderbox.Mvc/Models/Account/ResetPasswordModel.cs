using Orderbox.Core.Resources.Account;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Models.Account
{
    public class ResetPasswordModel
    {
        public string Token { get; set; }

        [Required]
        [Display(Name = "NewPassword", ResourceType = typeof(ResetPasswordResource))]
        public string NewPassword { get; set; }
    }
}
