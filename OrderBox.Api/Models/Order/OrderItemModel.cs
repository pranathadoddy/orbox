using System.ComponentModel.DataAnnotations;

namespace Orderbox.Api.Models.Order
{
    public class OrderItemModel
    {
        [Required]
        public ulong ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Note { get; set; }

        public string ProductName { get; set; }

        public string ProductImageUrl { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public string Unit { get; set; }
    }
}
