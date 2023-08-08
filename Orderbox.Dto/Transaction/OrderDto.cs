using Framework.Dto;
using Orderbox.Dto.Common;
using System;
using System.Collections.Generic;

namespace Orderbox.Dto.Transaction
{
    public class OrderDto : TenantAuditableDto<ulong>
    {
        #region Fields

        private ICollection<OrderItemDto> _orderItems;

        private ICollection<OrderAdditionalChargeDto> _orderAdditioinalCharge;

        #endregion

        #region Properties

        public ulong? CustomerId { get; set; }

        public string BuyerName { get; set; }

        public string BuyerPhoneNumber { get; set; }

        public string BuyerEmailAddress { get; set; }

        public string Description { get; set; }

        public string OrderNumber { get; set; }

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

        public DateTime PaidAt { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public TenantDto Tenant { get; set; }

        public ICollection<OrderItemDto> OrderItems 
        { 
            get { return this._orderItems ?? (this._orderItems = new List<OrderItemDto>()); } 

            set { this._orderItems = value; }
        }

        public ICollection<OrderAdditionalChargeDto> OrderAdditionalCharge
        {
            get { return this._orderAdditioinalCharge ?? (this._orderAdditioinalCharge = new List<OrderAdditionalChargeDto>()); }

            set { this._orderAdditioinalCharge = value; }
        }

        #endregion
    }
}