using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Agent.Models.Country
{
    public class CreateModel
    {
        [Required]
        [Display(Name = "Country", ResourceType = typeof(LocationResource))]
        public string Name { get; set; }
    }
}
