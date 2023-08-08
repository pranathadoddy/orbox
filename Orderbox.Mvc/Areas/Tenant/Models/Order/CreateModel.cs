using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Tenant.Models.Order
{
    public class CreateModel
    {
        public List<OrderItemModel> OrderItems { get; set; }

        [Required]
        public ulong TenantId { get; set; }

        [Required]
        public string BuyerName { get; set; }

        [Required]
        public string BuyerEmail { get; set; }

        [Required]
        public string BuyerPhone { get; set; }
        
        public string Description { get; set; }

        [Required]
        public ulong PaymentMethodId { get; set; }

        [Required]
        public string CaptchaToken { get; set; }
    }
}
