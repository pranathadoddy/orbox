using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComAgencyCategory
    {
        public ComAgencyCategory()
        {
            ComProductAgencyCategories = new HashSet<ComProductAgencyCategory>();
        }

        public ulong Id { get; set; }
        public ulong AgencyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool IsMainCategory { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComAgency Agency { get; set; }
        public virtual ICollection<ComProductAgencyCategory> ComProductAgencyCategories { get; set; }
    }
}
