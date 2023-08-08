using Microsoft.AspNetCore.Identity;
using Orderbox.Dto.Common;
using System.Collections.Generic;

namespace Orderbox.Dto.Authentication
{
    public class ApplicationUserDto: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicture { get; set; }

        public string TimeZone { get; set; }

        public bool IsActive { get; set; }

        public ICollection<TenantDto> Tenant { get; set; }
    }
}
