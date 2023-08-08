using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Administrator.Models.Agency
{
    public class EditModel
    {
        public ulong Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(AgencyResource))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(AgencyResource))]
        public string Description { get; set; }
    }
}
