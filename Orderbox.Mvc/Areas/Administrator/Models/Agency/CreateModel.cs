using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Administrator.Models.Agency
{
    public class CreateModel
    {
        [Required]
        [Display(Name = "Name", ResourceType = typeof(AgencyResource))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(AgencyResource))]
        public string Description { get; set; }
    }
}
