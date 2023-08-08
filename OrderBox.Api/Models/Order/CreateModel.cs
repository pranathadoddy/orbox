using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Api.Models.Order
{
    public class CreateModel
    {
        public List<OrderItemModel> OrderItems { get; set; }

        [Required]
        public ulong TenantId { get; set; }

        [Required]
        public ulong PaymentMethodId { get; set; }
        
        public string Description { get; set; }

        [Required]
        public string CaptchaToken { get; set; }
    }
}
