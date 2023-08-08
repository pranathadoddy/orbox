using Microsoft.AspNetCore.Mvc.Rendering;
using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Administrator.Models.Agent
{
    public class CreateModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(AgentResource))]
        public string Email { get; set; }
        public ulong AgencyId { get; set; }

        [Required]
        [Display(Name = "Privilege", ResourceType = typeof(AgentResource))]
        public string Privilege { get; set; }

        public SelectList Privileges { get; set; }
    }
}
