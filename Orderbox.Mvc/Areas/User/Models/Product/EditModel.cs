using Microsoft.AspNetCore.Mvc.Rendering;
using Orderbox.Core.Resources.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.User.Models.Product
{
    public class EditModel
    {
        public ulong Id { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(ProductResource))]
        public ulong CategoryId { get; set; }

        public SelectList Categories { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ProductResource))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ProductResource))]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price", ResourceType = typeof(ProductResource))]
        public decimal Price { get; set; }

        [Display(Name = "Discount", ResourceType = typeof(ProductResource))]
        public decimal? Discount { get; set; }

        [Required]
        [Display(Name = "Unit", ResourceType = typeof(ProductResource))]
        public string Unit { get; set; }

        [Required]
        [Display(Name = "IsAvailable", ResourceType = typeof(ProductResource))]
        public bool IsAvailable { get; set; }

        public List<ProductImageModel> ProductImages { get; set; }
    }
}
