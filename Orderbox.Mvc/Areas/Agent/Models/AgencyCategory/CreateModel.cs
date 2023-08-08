using System.ComponentModel.DataAnnotations;
using Orderbox.Core.Resources.Common;

namespace Orderbox.Mvc.Areas.Agent.Models.AgencyCategory
{
    public class CreateModel
    {

        [Required]
        [Display(Name = "Name", ResourceType = typeof(AgencyCategoryResource))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(AgencyCategoryResource))]
        public string Description { get; set; }

        [Display(Name = "IsMainCategory", ResourceType = typeof(AgencyCategoryResource))]
        public bool IsMainCategory { get; set; }

        [Required]
        public string Base64File { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
