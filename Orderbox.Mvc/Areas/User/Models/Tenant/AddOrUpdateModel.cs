using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.User.Models.Tenant
{
    public class AddOrUpdateModel
    {
        [Required]
        public string Token { get; set; }
    }
}
