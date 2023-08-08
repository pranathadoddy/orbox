using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class VchCustomerVoucher
    {
        public ulong Id { get; set; }
        public ulong CustomerId { get; set; }
        public ulong VoucherId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComCustomer Customer { get; set; }
        public virtual VchVoucher Voucher { get; set; }
    }
}
