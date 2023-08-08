using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class TrxOrder
    {
        public TrxOrder()
        {
            TrxOrderAdditionalCharges = new HashSet<TrxOrderAdditionalCharge>();
            TrxOrderItems = new HashSet<TrxOrderItem>();
        }

        public ulong Id { get; set; }
        public string OrderNumber { get; set; }
        public ulong TenantId { get; set; }
        public ulong? CustomerId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerPhoneNumber { get; set; }
        public string BuyerEmailAddress { get; set; }
        public string Description { get; set; }
        public string PaymentOptionCode { get; set; }
        public string PaymentProviderName { get; set; }
        public string PaymentAccountName { get; set; }
        public string PaymentAccountNumber { get; set; }
        public string PaymentDescription { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentGatewayInvoiceUrl { get; set; }
        public string PaymentProof { get; set; }
        public string PaymentChannel { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComCustomer Customer { get; set; }
        public virtual ComTenant Tenant { get; set; }
        public virtual ICollection<TrxOrderAdditionalCharge> TrxOrderAdditionalCharges { get; set; }
        public virtual ICollection<TrxOrderItem> TrxOrderItems { get; set; }
    }
}
