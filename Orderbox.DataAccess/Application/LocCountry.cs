using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class LocCountry
    {
        public LocCountry()
        {
            LocCities = new HashSet<LocCity>();
        }

        public ulong Id { get; set; }
        public ulong AgencyId { get; set; }
        public string Name { get; set; }
        public string Flag { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComAgency Agency { get; set; }
        public virtual ICollection<LocCity> LocCities { get; set; }
    }
}
