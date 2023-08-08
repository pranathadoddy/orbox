ALTER TABLE com_Tenant
ADD `CustomerMustLogin` bit(1) NOT NULL AFTER `IsActive`;
ALTER TABLE com_Tenant
ADD `GoogleOAuthClientId` text NULL AFTER `CustomerMustLogin`;
ALTER TABLE com_Tenant
ADD `CheckoutForm` varchar(32) NULL AFTER `GoogleOAuthClientId`;
ALTER TABLE com_Tenant
ADD `AgencyId` bigint unsigned NULL AFTER `CheckoutForm`;
ALTER TABLE com_Tenant
ADD `EnableShop` bit NOT NULL AFTER `AgencyId`;
ALTER TABLE com_Tenant
ADD CONSTRAINT `fk_com_Tenant_com_Agency` FOREIGN KEY (`AgencyId`) REFERENCES `com_Agency` (`Id`) ON DELETE CASCADE;