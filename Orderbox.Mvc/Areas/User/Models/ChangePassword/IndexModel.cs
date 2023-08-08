using Orderbox.Core.Resources.Account;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.User.Models.ChangePassword
{
    public class IndexModel
    {
        [Required]
        [Display(Name = "OldPassword", ResourceType = typeof(ChangePasswordResource))]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "NewPassword", ResourceType = typeof(ChangePasswordResource))]
        public string NewPassword { get; set; }

    }
}
