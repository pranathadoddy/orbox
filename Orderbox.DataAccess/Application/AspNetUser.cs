using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            ComTenants = new HashSet<ComTenant>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string TimeZone { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<ComTenant> ComTenants { get; set; }
    }
}
