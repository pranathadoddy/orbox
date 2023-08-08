using Microsoft.AspNetCore.Mvc.Rendering;
using Orderbox.Core.Resources.Account;
using Orderbox.Core.Resources.Common;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Models.Account
{
    public class ActivationModel
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

        [Required]
        [Display(Name = "BusinessName", ResourceType = typeof(RegistrationResource))]
        public string BusinessName { get; set; }

        [Required]
        [Display(Name = "TenantDomain", Description = "TenantDomain_Description", ResourceType = typeof(RegistrationResource))]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9-]{1,18}[a-zA-Z0-9]$", ErrorMessageResourceName = "SubDomainErrorFormatMessage", ErrorMessageResourceType = typeof(RegistrationResource))]
        public string TenantDomain { get; set; }

        public string DomainPostfix { get; set; }

        [Display(Name = "Country", ResourceType = typeof(TenantResource))]
        public string CountryId { get; set; }

        public SelectList Countries { get; set; }

        public string AreaCode { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(TenantResource))]
        public string Phone { get; set; }
    }
}
