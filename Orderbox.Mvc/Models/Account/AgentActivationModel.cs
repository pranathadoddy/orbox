using Orderbox.Core.Resources.Account;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Models.Account
{
    public class AgentActivationModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [Display(Name = "FirstName", ResourceType = typeof(RegistrationResource))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(RegistrationResource))]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(RegistrationResource))]
        public string Password { get; set; }
    }
}
