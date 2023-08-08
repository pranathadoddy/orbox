using System.ComponentModel.DataAnnotations;
using Orderbox.Core.Resources.Common;

namespace Orderbox.Mvc.Areas.Agent.Models.AgencyCategory
{
    public class EditModel
    {
        public ulong Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(AgencyCategoryResource))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(AgencyCategoryResource))]
        public string Description { get; set; }

        [Display(Name = "IsMainCategory", ResourceType = typeof(AgencyCategoryResource))]
        public bool IsMainCategory { get; set; }

        public string Base64File { get; set; }

        public string FileName { get; set; }

        public string IconUrl { get; set; }
    }
}
