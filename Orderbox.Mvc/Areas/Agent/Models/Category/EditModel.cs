using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Agent.Models.Category
{
    public class EditModel
    {
        public string MerchantName { get; set; }

        public SideNavigationModel SideNavigation { get; set; }

        public ulong Id { get; set; }

        public ulong TenantId { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(CategoryResource))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(CategoryResource))]
        public string Description { get; set; }
    }
}
