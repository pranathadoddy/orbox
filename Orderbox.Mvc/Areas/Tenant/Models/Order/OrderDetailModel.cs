using System.Collections.Generic;

namespace Orderbox.Mvc.Areas.Tenant.Models.Order
{
    public class OrderDetailModel
    {
        public ulong OrderId { get; set; }

        public string TenantLogoUrl { get; set; }

        public string TenantName { get; set; }

        public string TenantPhoneAreaCode { get; set; }

        public string TenantPhone { get; set; }

        public bool TenantUnderAgency { get; set; }

        public string Currency { get; set; }

        public string OrderNumber { get; set; }

        public string BuyerName { get; set; }

        public string OrderStatus { get; set; }

        public string PaymentOptionCode { get; set; }

        public string PaymentProviderName { get; set; }

        public string PaymentAccountName { get; set; }

        public string PaymentAccountNumber { get; set; }

        public string PaymentDescription { get; set; }

        public string PaymentStatus { get; set; }

        public string PaymentGatewayInvoiceUrl { get; set; }

        public string PaymentProof { get; set; }

        public List<OrderItemModel> OrderItems { get; set; }
    }
}
