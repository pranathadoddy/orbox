SET character_set_client = utf8mb4 ;
CREATE TABLE `loc_Store` (
  `Id` bigint(20) unsigned PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `TenantId` bigint(20) unsigned NOT NULL,
  `CityId` bigint(20) unsigned NOT NULL,
  `Name` varchar(256) NOT NULL,
  `Address` text NULL,
  `MapUrl` text NULL,
  `CreatedBy` varchar(256) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `LastModifiedBy` varchar(256) NOT NULL,
  `LastModifiedDateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE loc_Store
ADD CONSTRAINT `fk_loc_Store_com_Tenant` FOREIGN KEY (`TenantId`) REFERENCES `com_Tenant` (`Id`) ON DELETE CASCADE;
ALTER TABLE loc_Store
ADD CONSTRAINT `fk_loc_Store_loc_City` FOREIGN KEY (`CityId`) REFERENCES `loc_City` (`Id`) ON DELETE CASCADE;