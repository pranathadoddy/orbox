using System;
using System.Collections.Generic;

namespace Orderbox.DataAccess.Application
{
    public partial class ComTenant
    {
        public ComTenant()
        {
            ComCategories = new HashSet<ComCategory>();
            ComPayments = new HashSet<ComPayment>();
            ComProductAgencyCategories = new HashSet<ComProductAgencyCategory>();
            ComProductImages = new HashSet<ComProductImage>();
            ComProductStores = new HashSet<ComProductStore>();
            ComProductSubCategories = new HashSet<ComProductSubCategory>();
            ComProducts = new HashSet<ComProduct>();
            ComSubCategories = new HashSet<ComSubCategory>();
            ComTenantPushNotificationTokens = new HashSet<ComTenantPushNotificationToken>();
            LocStores = new HashSet<LocStore>();
            TrxOrderAdditionalCharges = new HashSet<TrxOrderAdditionalCharge>();
            TrxOrderItems = new HashSet<TrxOrderItem>();
            TrxOrders = new HashSet<TrxOrder>();
        }

        public ulong Id { get; set; }
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
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ComAgency Agency { get; set; }
        public virtual AspNetUser User { get; set; }
        public virtual ICollection<ComCategory> ComCategories { get; set; }
        public virtual ICollection<ComPayment> ComPayments { get; set; }
        public virtual ICollection<ComProductAgencyCategory> ComProductAgencyCategories { get; set; }
        public virtual ICollection<ComProductImage> ComProductImages { get; set; }
        public virtual ICollection<ComProductStore> ComProductStores { get; set; }
        public virtual ICollection<ComProductSubCategory> ComProductSubCategories { get; set; }
        public virtual ICollection<ComProduct> ComProducts { get; set; }
        public virtual ICollection<ComSubCategory> ComSubCategories { get; set; }
        public virtual ICollection<ComTenantPushNotificationToken> ComTenantPushNotificationTokens { get; set; }
        public virtual ICollection<LocStore> LocStores { get; set; }
        public virtual ICollection<TrxOrderAdditionalCharge> TrxOrderAdditionalCharges { get; set; }
        public virtual ICollection<TrxOrderItem> TrxOrderItems { get; set; }
        public virtual ICollection<TrxOrder> TrxOrders { get; set; }
    }
}
