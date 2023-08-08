using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Agent.Models.Merchant
{
    public class EditModel
    {
        public SideNavigationModel SideNavigation { get; set; }

        [Required]
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        [Display(Name = "ShopActive", ResourceType = typeof(TenantResource))]
        public bool ShopEnabled { get; set; }

        [Display(Name = "AllowToAccessCategory", ResourceType = typeof(TenantResource))]
        public bool AllowToAccessCategory { get; set; }

        [Display(Name = "AllowToAccessProduct", ResourceType = typeof(TenantResource))]
        public bool AllowToAccessProduct { get; set; }

        [Display(Name = "AllowToAccessProfile", ResourceType = typeof(TenantResource))]
        public bool AllowToAccessProfile { get; set; }

        [Display(Name = "AllowToAccessCheckoutSetting", ResourceType = typeof(TenantResource))]
        public bool AllowToAccessCheckoutSetting { get; set; }
    }
}
