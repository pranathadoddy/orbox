using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComCustomer
    {
        public ComCustomer()
        {
            TrxOrders = new HashSet<TrxOrder>();
            VchCustomerVouchers = new HashSet<VchCustomerVoucher>();
        }

        public ulong Id { get; set; }
        public string AuthType { get; set; }
        public string ExternalId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string ProfilePicture { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<TrxOrder> TrxOrders { get; set; }
        public virtual ICollection<VchCustomerVoucher> VchCustomerVouchers { get; set; }
    }
}
