using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class LocCity
    {
        public LocCity()
        {
            LocStores = new HashSet<LocStore>();
        }

        public ulong Id { get; set; }
        public ulong AgencyId { get; set; }
        public ulong CountryId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComAgency Agency { get; set; }
        public virtual LocCountry Country { get; set; }
        public virtual ICollection<LocStore> LocStores { get; set; }
    }
}
