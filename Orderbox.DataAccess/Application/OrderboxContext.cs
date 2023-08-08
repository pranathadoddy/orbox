using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Orderbox.DataAccess.Application
{
    public partial class OrderboxContext : DbContext
    {
        public OrderboxContext()
        {
        }

        public OrderboxContext(DbContextOptions<OrderboxContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<ComAgency> ComAgencies { get; set; }
        public virtual DbSet<ComAgencyCategory> ComAgencyCategories { get; set; }
        public virtual DbSet<ComAgent> ComAgents { get; set; }
        public virtual DbSet<ComCategory> ComCategories { get; set; }
        public virtual DbSet<ComCustomer> ComCustomers { get; set; }
        public virtual DbSet<ComPayment> ComPayments { get; set; }
        public virtual DbSet<ComProduct> ComProducts { get; set; }
        public virtual DbSet<ComProductAgencyCategory> ComProductAgencyCategories { get; set; }
        public virtual DbSet<ComProductImage> ComProductImages { get; set; }
        public virtual DbSet<ComProductStore> ComProductStores { get; set; }
        public virtual DbSet<ComProductSubCategory> ComProductSubCategories { get; set; }
        public virtual DbSet<ComRegistration> ComRegistrations { get; set; }
        public virtual DbSet<ComSubCategory> ComSubCategories { get; set; }
        public virtual DbSet<ComTenant> ComTenants { get; set; }
        public virtual DbSet<ComTenantPushNotificationToken> ComTenantPushNotificationTokens { get; set; }
        public virtual DbSet<LocCity> LocCities { get; set; }
        public virtual DbSet<LocCountry> LocCountries { get; set; }
        public virtual DbSet<LocStore> LocStores { get; set; }
        public virtual DbSet<TrxOrder> TrxOrders { get; set; }
        public virtual DbSet<TrxOrderAdditionalCharge> TrxOrderAdditionalCharges { get; set; }
        public virtual DbSet<TrxOrderItem> TrxOrderItems { get; set; }
        public virtual DbSet<VchCustomerVoucher> VchCustomerVouchers { get; set; }
        public virtual DbSet<VchVoucher> VchVouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.LockoutEnd).HasMaxLength(6);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.TimeZone).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<ComAgency>(entity =>
            {
                entity.ToTable("com_Agency");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<ComAgencyCategory>(entity =>
            {
                entity.ToTable("com_AgencyCategory");

                entity.HasIndex(e => e.AgencyId, "fk_com_AgencyCategory_com_Agency");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Icon).HasColumnType("text");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Agency)
                    .WithMany(p => p.ComAgencyCategories)
                    .HasForeignKey(d => d.AgencyId)
                    .HasConstraintName("fk_com_AgencyCategory_com_Agency");
            });

            modelBuilder.Entity<ComAgent>(entity =>
            {
                entity.ToTable("com_Agent");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Privilege)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserId).HasMaxLength(255);
            });

            modelBuilder.Entity<ComCategory>(entity =>
            {
                entity.ToTable("com_Category");

                entity.HasIndex(e => e.TenantId, "fk_com_Category_com_Tenant1");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ComCategories)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_com_Category_com_Tenant1");
            });

            modelBuilder.Entity<ComCustomer>(entity =>
            {
                entity.ToTable("com_Customer");

                entity.Property(e => e.AuthType)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ExternalId)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(256);

                entity.Property(e => e.Phone).HasMaxLength(32);

                entity.Property(e => e.ProfilePicture).HasColumnType("text");
            });

            modelBuilder.Entity<ComPayment>(entity =>
            {
                entity.ToTable("com_Payment");

                entity.HasIndex(e => e.TenantId, "fk_com_Payment_com_Tenant1");

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ApiKey).HasColumnType("text");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.PaymentOptionCode)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.ProviderName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.WebhookValidationSecret).HasColumnType("text");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ComPayments)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_com_Payment_com_Tenant1");
            });

            modelBuilder.Entity<ComProduct>(entity =>
            {
                entity.ToTable("com_Product");

                entity.HasIndex(e => e.CategoryId, "fk_com_Product_com_Category1");

                entity.HasIndex(e => e.TenantId, "fk_com_Product_com_Tenant1");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Discount)
                    .HasPrecision(5, 2)
                    .HasDefaultValueSql("'0.00'");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Price).HasPrecision(10);

                entity.Property(e => e.RedeemMethod).HasMaxLength(32);

                entity.Property(e => e.TermAndCondition).HasColumnType("text");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.ValidPeriodEnd).HasColumnType("datetime");

                entity.Property(e => e.ValidPeriodStart).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ComProducts)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_com_Product_com_Category1");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ComProducts)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_com_Product_com_Tenant1");
            });

            modelBuilder.Entity<ComProductAgencyCategory>(entity =>
            {
                entity.ToTable("com_ProductAgencyCategory");

                entity.HasIndex(e => e.AgencyId, "fk_com_ProductAgencyCategory_com_Agency");

                entity.HasIndex(e => e.AgencyCategoryId, "fk_com_ProductAgencyCategory_com_AgencyCategory");

                entity.HasIndex(e => e.ProductId, "fk_com_ProductAgencyCategory_com_Product");

                entity.HasIndex(e => e.TenantId, "fk_com_ProductAgencyCategory_com_Tenant");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.AgencyCategory)
                    .WithMany(p => p.ComProductAgencyCategories)
                    .HasForeignKey(d => d.AgencyCategoryId)
                    .HasConstraintName("fk_com_ProductAgencyCategory_com_AgencyCategory");

                entity.HasOne(d => d.Agency)
                    .WithMany(p => p.ComProductAgencyCategories)
                    .HasForeignKey(d => d.AgencyId)
                    .HasConstraintName("fk_com_ProductAgencyCategory_com_Agency");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ComProductAgencyCategories)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_com_ProductAgencyCategory_com_Product");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ComProductAgencyCategories)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_com_ProductAgencyCategory_com_Tenant");
            });

            modelBuilder.Entity<ComProductImage>(entity =>
            {
                entity.ToTable("com_ProductImage");

                entity.HasIndex(e => e.ProductId, "fk_com_ProductImage_com_Product1");

                entity.HasIndex(e => e.TenantId, "fk_com_ProductImage_com_Tenant1");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ComProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_com_ProductImage_com_Product1");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ComProductImages)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_com_ProductImage_com_Tenant1");
            });

            modelBuilder.Entity<ComProductStore>(entity =>
            {
                entity.ToTable("com_ProductStore");

                entity.HasIndex(e => e.ProductId, "fk_com_ProductStore_com_Product");

                entity.HasIndex(e => e.TenantId, "fk_com_ProductStore_com_Tenant");

                entity.HasIndex(e => e.StoreId, "fk_com_ProductStore_loc_Store");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ComProductStores)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_com_ProductStore_com_Product");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ComProductStores)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("fk_com_ProductStore_loc_Store");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ComProductStores)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_com_ProductStore_com_Tenant");
            });

            modelBuilder.Entity<ComProductSubCategory>(entity =>
            {
                entity.ToTable("com_ProductSubCategory");

                entity.HasIndex(e => e.ProductId, "fk_com_ProductSubCategory_com_Product");

                entity.HasIndex(e => e.SubCategoryId, "fk_com_ProductSubCategory_com_SubCategory");

                entity.HasIndex(e => e.TenantId, "fk_com_ProductSubCategory_com_Tenant");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ComProductSubCategories)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_com_ProductSubCategory_com_Product");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.ComProductSubCategories)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("fk_com_ProductSubCategory_com_SubCategory");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ComProductSubCategories)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_com_ProductSubCategory_com_Tenant");
            });

            modelBuilder.Entity<ComRegistration>(entity =>
            {
                entity.ToTable("com_Registration");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.VerificationCode)
                    .IsRequired()
                    .HasMaxLength(8);
            });

            modelBuilder.Entity<ComSubCategory>(entity =>
            {
                entity.ToTable("com_SubCategory");

                entity.HasIndex(e => e.CategoryId, "fk_com_SubCategory_com_Category");

                entity.HasIndex(e => e.TenantId, "fk_com_SubCategory_com_Tenant");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ComSubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_com_SubCategory_com_Category");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ComSubCategories)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_com_SubCategory_com_Tenant");
            });

            modelBuilder.Entity<ComTenant>(entity =>
            {
                entity.ToTable("com_Tenant");

                entity.HasIndex(e => e.AgencyId, "fk_com_Tenant_com_Agency");

                entity.HasIndex(e => e.UserId, "fk_com_Tenant_to_AspNetUsers1");

                entity.Property(e => e.AdditionalInformation).HasMaxLength(256);

                entity.Property(e => e.Address).HasMaxLength(1024);

                entity.Property(e => e.CheckoutForm).HasMaxLength(32);

                entity.Property(e => e.CountryCode).HasMaxLength(8);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasDefaultValueSql("'RP'");

                entity.Property(e => e.CustomDomain).HasMaxLength(256);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.GoogleOauthClientId)
                    .HasColumnType("text")
                    .HasColumnName("GoogleOAuthClientId");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Logo).HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.OrderboxDomain)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Phone).HasMaxLength(128);

                entity.Property(e => e.PhoneAreaCode).HasMaxLength(8);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Wallpaper).HasMaxLength(1024);

                entity.HasOne(d => d.Agency)
                    .WithMany(p => p.ComTenants)
                    .HasForeignKey(d => d.AgencyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_com_Tenant_com_Agency");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ComTenants)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_com_Tenant_to_AspNetUsers1");
            });

            modelBuilder.Entity<ComTenantPushNotificationToken>(entity =>
            {
                entity.ToTable("com_TenantPushNotificationToken");

                entity.HasIndex(e => e.TenantId, "fk_com_TenantPushNotificationToken_com_Tenant1");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.ComTenantPushNotificationTokens)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_com_TenantPushNotificationToken_com_Tenant1");
            });

            modelBuilder.Entity<LocCity>(entity =>
            {
                entity.ToTable("loc_City");

                entity.HasIndex(e => e.AgencyId, "fk_loc_City_com_Agency");

                entity.HasIndex(e => e.CountryId, "fk_loc_City_loc_Country");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Agency)
                    .WithMany(p => p.LocCities)
                    .HasForeignKey(d => d.AgencyId)
                    .HasConstraintName("fk_loc_City_com_Agency");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.LocCities)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("fk_loc_City_loc_Country");
            });

            modelBuilder.Entity<LocCountry>(entity =>
            {
                entity.ToTable("loc_Country");

                entity.HasIndex(e => e.AgencyId, "fk_loc_Country_com_Agency");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Flag).HasMaxLength(255);

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Agency)
                    .WithMany(p => p.LocCountries)
                    .HasForeignKey(d => d.AgencyId)
                    .HasConstraintName("fk_loc_Country_com_Agency");
            });

            modelBuilder.Entity<LocStore>(entity =>
            {
                entity.ToTable("loc_Store");

                entity.HasIndex(e => e.TenantId, "fk_loc_Store_com_Tenant");

                entity.HasIndex(e => e.CityId, "fk_loc_Store_loc_City");

                entity.Property(e => e.Address).HasColumnType("text");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.MapUrl).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.LocStores)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("fk_loc_Store_loc_City");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.LocStores)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_loc_Store_com_Tenant");
            });

            modelBuilder.Entity<TrxOrder>(entity =>
            {
                entity.ToTable("trx_Order");

                entity.HasIndex(e => e.CustomerId, "fk_trx_Order_com_Customer");

                entity.HasIndex(e => e.TenantId, "fk_trx_Order_com_Tenant");

                entity.Property(e => e.BuyerEmailAddress)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.BuyerName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.BuyerPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.OrderNumber).HasMaxLength(500);

                entity.Property(e => e.PaidAt).HasColumnType("datetime");

                entity.Property(e => e.PaymentAccountName).HasMaxLength(256);

                entity.Property(e => e.PaymentAccountNumber).HasMaxLength(256);

                entity.Property(e => e.PaymentChannel).HasMaxLength(256);

                entity.Property(e => e.PaymentDescription).HasColumnType("text");

                entity.Property(e => e.PaymentGatewayInvoiceUrl).HasColumnType("text");

                entity.Property(e => e.PaymentMethod).HasMaxLength(256);

                entity.Property(e => e.PaymentOptionCode).HasMaxLength(5);

                entity.Property(e => e.PaymentProof).HasColumnType("text");

                entity.Property(e => e.PaymentProviderName).HasMaxLength(256);

                entity.Property(e => e.PaymentStatus)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TrxOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_trx_Order_com_Customer");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.TrxOrders)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_trx_Order_com_Tenant");
            });

            modelBuilder.Entity<TrxOrderAdditionalCharge>(entity =>
            {
                entity.ToTable("trx_OrderAdditionalCharge");

                entity.HasIndex(e => e.TenantId, "fk_trx_OrderAdditionalCharge_com_Tenant");

                entity.HasIndex(e => e.OrderId, "fk_trx_OrderAdditionalCharge_trx_Order");

                entity.Property(e => e.Amount).HasPrecision(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TrxOrderAdditionalCharges)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("fk_trx_OrderAdditionalCharge_trx_Order");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.TrxOrderAdditionalCharges)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_trx_OrderAdditionalCharge_com_Tenant");
            });

            modelBuilder.Entity<TrxOrderItem>(entity =>
            {
                entity.ToTable("trx_OrderItem");

                entity.HasIndex(e => e.TenantId, "fk_trx_OrderItem_com_Tenant");

                entity.HasIndex(e => e.OrderId, "fk_trx_OrderItem_trx_Order");

                entity.Property(e => e.AvailableRedeemStores).HasColumnType("text");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Discount)
                    .HasPrecision(5, 2)
                    .HasDefaultValueSql("'0.00'");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Note).HasColumnType("text");

                entity.Property(e => e.ProductDescription).HasColumnType("text");

                entity.Property(e => e.ProductImage).HasColumnType("text");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ProductRedeemMethod).HasMaxLength(32);

                entity.Property(e => e.ProductTermAndCondition).HasColumnType("text");

                entity.Property(e => e.ProductType).HasMaxLength(256);

                entity.Property(e => e.ProductUnit)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.UnitPrice).HasPrecision(10);

                entity.Property(e => e.ValidEndDate).HasColumnType("datetime");

                entity.Property(e => e.ValidStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TrxOrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("fk_trx_OrderItem_trx_Order");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.TrxOrderItems)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("fk_trx_OrderItem_com_Tenant");
            });

            modelBuilder.Entity<VchCustomerVoucher>(entity =>
            {
                entity.ToTable("vch_CustomerVoucher");

                entity.HasIndex(e => e.CustomerId, "fk_vch_CustomerVoucher_com_Customer");

                entity.HasIndex(e => e.VoucherId, "fk_vch_CustomerVoucher_vch_Voucher");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.VchCustomerVouchers)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("fk_vch_CustomerVoucher_com_Customer");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.VchCustomerVouchers)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("fk_vch_CustomerVoucher_vch_Voucher");
            });

            modelBuilder.Entity<VchVoucher>(entity =>
            {
                entity.ToTable("vch_Voucher");

                entity.HasIndex(e => e.OrderItemId, "fk_vch_Voucher_com_OrderItem");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.LastModifiedBy)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.LastModifiedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RedeemDate).HasColumnType("datetime");

                entity.Property(e => e.RedeemMethod).HasMaxLength(32);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.TermAndCondition).HasColumnType("text");

                entity.Property(e => e.ValidEndDate).HasColumnType("datetime");

                entity.Property(e => e.ValidStartDate).HasColumnType("datetime");

                entity.Property(e => e.VoucherCode)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.OrderItem)
                    .WithMany(p => p.VchVouchers)
                    .HasForeignKey(d => d.OrderItemId)
                    .HasConstraintName("fk_vch_Voucher_com_OrderItem");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
