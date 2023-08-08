using Orderbox.Core.Resources.Account;
using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.User.Models.Category
{
    public class EditModel
    {
        public ulong Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(CategoryResource))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(CategoryResource))]
        public string Description { get; set; }
    }
}
