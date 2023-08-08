using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Agent.Models.Country
{
    public class EditModel
    {
        public ulong Id { get; set; }

        [Required]
        [Display(Name = "Country", ResourceType = typeof(LocationResource))]
        public string Name { get; set; }

        public bool HasCity { get; set; }
    }
}
