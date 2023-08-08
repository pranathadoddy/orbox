SET character_set_client = utf8mb4 ;
CREATE TABLE `com_ProductAgencyCategory` (
  `Id` bigint(20) unsigned PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `AgencyId` bigint(20) unsigned NOT NULL,
  `TenantId` bigint(20) unsigned NOT NULL,
  `ProductId` bigint(20) unsigned NOT NULL,
  `AgencyCategoryId` bigint(20) unsigned NOT NULL,
  `CreatedBy` varchar(256) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `LastModifiedBy` varchar(256) NOT NULL,
  `LastModifiedDateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE com_ProductAgencyCategory
ADD CONSTRAINT `fk_com_ProductAgencyCategory_com_Agency` FOREIGN KEY (`AgencyId`) REFERENCES `com_Agency` (`Id`) ON DELETE CASCADE;
ALTER TABLE com_ProductAgencyCategory
ADD CONSTRAINT `fk_com_ProductAgencyCategory_com_Tenant` FOREIGN KEY (`TenantId`) REFERENCES `com_Tenant` (`Id`) ON DELETE CASCADE;
ALTER TABLE com_ProductAgencyCategory
ADD CONSTRAINT `fk_com_ProductAgencyCategory_com_AgencyCategory` FOREIGN KEY (`AgencyCategoryId`) REFERENCES `com_AgencyCategory` (`Id`) ON DELETE CASCADE;
ALTER TABLE com_ProductAgencyCategory
ADD CONSTRAINT `fk_com_ProductAgencyCategory_com_Product` FOREIGN KEY (`ProductId`) REFERENCES `com_Product`(`Id`) ON DELETE CASCADE;