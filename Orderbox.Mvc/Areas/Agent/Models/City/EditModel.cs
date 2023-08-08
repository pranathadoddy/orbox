using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Agent.Models.City
{
    public class EditModel
    {
        public SideNavigationModel SideNavigation { get; set; }

        public ulong Id { get; set; }

        public ulong CountryId { get; set; }

        [Required]
        [Display(Name = "City", ResourceType = typeof(LocationResource))]
        public string Name { get; set; }

        public bool HasStore { get; set; }
    }
}
