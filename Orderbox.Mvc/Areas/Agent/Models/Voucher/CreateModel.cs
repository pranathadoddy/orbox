using Microsoft.AspNetCore.Mvc.Rendering;
using Orderbox.Core.Resources.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Agent.Models.Voucher
{
    public class CreateModel
    {
        public ulong TenantId { get; set; }

        public string MerchantName { get; set; }

        public SideNavigationModel SideNavigation { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(ProductResource))]
        public ulong CategoryId { get; set; }

        public SelectList Categories { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ProductResource))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ProductResource))]
        public string Description { get; set; }
        [Display(Name = "Discount", ResourceType = typeof(ProductResource))]
        public decimal? Discount { get; set; }

        [Required]
        [Display(Name = "Price", ResourceType = typeof(ProductResource))]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Unit", ResourceType = typeof(ProductResource))]
        public string Unit { get; set; }

        [Required]
        [Display(Name = "IsAvailable", ResourceType = typeof(ProductResource))]
        public bool IsAvailable { get; set; }

        [Required]
        [Display(Name = "ValidPeriodStart", ResourceType = typeof(ProductResource))]
        public DateTime ValidPeriodStart { get; set; }

        [Required]
        [Display(Name = "ValidPeriodEnd", ResourceType = typeof(ProductResource))]
        public DateTime ValidPeriodEnd { get; set; }

        [Required]
        [Display(Name = "TermAndCondition", ResourceType = typeof(ProductResource))]
        public string TermAndCondition { get; set; }

        [Required]
        [Display(Name = "Commision", ResourceType = typeof(ProductResource))]
        public double Commision { get; set; }

        [Required]
        [Display(Name = "RedeemMethod", ResourceType = typeof(ProductResource))]
        public string RedeemMethod { get; set; }

        public SelectList RedeemMethods { get; set; }

        [Required]
        public ICollection<ulong> AgencyCategoryIds { get; set; }

        [Required]
        public ICollection<ulong> StoreIds { get; set; }
    }
}
