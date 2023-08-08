using Microsoft.AspNetCore.Mvc.Rendering;
using Orderbox.Core.Resources.Account;
using Orderbox.Core.Resources.Common;
using Orderbox.Dto.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Orderbox.Mvc.Areas.User.Models.Profile
{
    public class IndexModel
    {
        #region Properties

        public string LogoUrl { get; set; }

        public string WallpaperUrl { get; set; }

        [Required]
        [Display(Name = "FirstName", ResourceType = typeof(RegistrationResource))]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(RegistrationResource))]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "BusinessName", ResourceType = typeof(RegistrationResource))]
        public string BusinessName { get; set; }

        [Display(Name = "Country", ResourceType = typeof(TenantResource))]
        public string CountryId { get; set; }

        public SelectList Countries { get; set; }

        [Display(Name = "Address", ResourceType = typeof(TenantResource))]
        public string Address { get; set; }

        public string AreaCode { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(TenantResource))]
        public string Phone { get; set; }

        [Display(Name = "AdditionalInformation", Description = "AdditionalInformationPlaceholder", ResourceType = typeof(TenantResource))]
        public string AdditionalInformation { get; set; }

        public ICollection<PaymentDto> PaymentDtos { get; set; }

        #endregion
    }
}
