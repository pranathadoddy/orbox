using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComAgency
    {
        public ComAgency()
        {
            ComAgencyCategories = new HashSet<ComAgencyCategory>();
            ComProductAgencyCategories = new HashSet<ComProductAgencyCategory>();
            ComTenants = new HashSet<ComTenant>();
            LocCities = new HashSet<LocCity>();
            LocCountries = new HashSet<LocCountry>();
        }

        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<ComAgencyCategory> ComAgencyCategories { get; set; }
        public virtual ICollection<ComProductAgencyCategory> ComProductAgencyCategories { get; set; }
        public virtual ICollection<ComTenant> ComTenants { get; set; }
        public virtual ICollection<LocCity> LocCities { get; set; }
        public virtual ICollection<LocCountry> LocCountries { get; set; }
    }
}
