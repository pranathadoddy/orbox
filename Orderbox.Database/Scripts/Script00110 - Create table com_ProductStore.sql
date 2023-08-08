SET character_set_client = utf8mb4 ;
CREATE TABLE `com_ProductStore` (
  `Id` bigint(20) unsigned PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `TenantId` bigint(20) unsigned NOT NULL,
  `ProductId` bigint(20) unsigned NOT NULL,
  `StoreId` bigint(20) unsigned NOT NULL,
  `CreatedBy` varchar(256) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `LastModifiedBy` varchar(256) NOT NULL,
  `LastModifiedDateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE com_ProductStore
ADD CONSTRAINT `fk_com_ProductStore_com_Tenant` FOREIGN KEY (`TenantId`) REFERENCES `com_Tenant` (`Id`) ON DELETE CASCADE;
ALTER TABLE com_ProductStore
ADD CONSTRAINT `fk_com_ProductStore_com_Product` FOREIGN KEY (`ProductId`) REFERENCES `com_Product` (`Id`) ON DELETE CASCADE;
ALTER TABLE com_ProductStore
ADD CONSTRAINT `fk_com_ProductStore_loc_Store` FOREIGN KEY (`StoreId`) REFERENCES `loc_Store` (`Id`) ON DELETE CASCADE;