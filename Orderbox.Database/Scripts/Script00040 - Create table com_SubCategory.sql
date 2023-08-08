SET character_set_client = utf8mb4 ;
CREATE TABLE `com_SubCategory` (
  `Id` bigint(20) unsigned PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `TenantId` bigint(20) unsigned NOT NULL,
  `CategoryId` bigint(20) unsigned NOT NULL,
  `Name` varchar(256) NOT NULL,
  `Description` text NULL,
  `CreatedBy` varchar(256) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `LastModifiedBy` varchar(256) NOT NULL,
  `LastModifiedDateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE com_SubCategory
ADD CONSTRAINT `fk_com_SubCategory_com_Tenant` FOREIGN KEY (`TenantId`) REFERENCES `com_Tenant` (`Id`) ON DELETE CASCADE;
ALTER TABLE com_SubCategory
ADD CONSTRAINT `fk_com_SubCategory_com_Category` FOREIGN KEY (`CategoryId`) REFERENCES `com_Category` (`Id`) ON DELETE CASCADE;