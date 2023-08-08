using Microsoft.AspNetCore.Mvc.Rendering;
using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.Administrator.Models.Agent
{
    public class ViewModel
    {
        public ulong Id { get; set; }

        [Editable(false)]
        [Display(Name = "Email", ResourceType = typeof(AgentResource))]
        public string Email { get; set; }

        public ulong AgencyId { get; set; }

        [Editable(false)]
        [Display(Name = "Privilege", ResourceType = typeof(AgentResource))]
        public string Privilege { get; set; }

        public SelectList Privileges { get; set; }
    }
}
