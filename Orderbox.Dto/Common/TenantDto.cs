using Framework.Dto;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Orderbox.Dto.Common
{
    public class TenantDto : AuditableDto<ulong>
    {
        #region Fields

        private ICollection<PaymentDto> _paymentDtos;

        #endregion

        #region Properties

        public string UserId { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string OrderboxDomain { get; set; }

        public string CustomDomain { get; set; }

        public string Address { get; set; }

        public string CountryCode { get; set; }
        
        public string PhoneAreaCode { get; set; }
        
        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public bool CustomerMustLogin { get; set; }

        public string GoogleOauthClientId { get; set; }

        public string CheckoutForm { get; set; }

        public ulong? AgencyId { get; set; }

        public bool EnableShop { get; set; }

        public bool AllowToAccessCategory { get; set; }

        public bool AllowToAccessProduct { get; set; }

        public bool AllowToAccessProfile { get; set; }

        public bool AllowToAccessCheckoutSetting { get; set; }

        public string Logo { get; set; }

        public string Wallpaper { get; set; }

        public string Currency { get; set; }

        public string AdditionalInformation { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public ICollection<PaymentDto> PaymentDtos
        {
            get { return this._paymentDtos ?? (this._paymentDtos = new List<PaymentDto>()); }
        }

        public TenantPushNotificationTokenDto TenantPushNotificationTokenDto { get; set; }

        #endregion
    }
}
