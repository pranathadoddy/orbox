SET character_set_client = utf8mb4 ;
CREATE TABLE `loc_City` (
  `Id` bigint(20) unsigned PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `AgencyId` bigint(20) unsigned NOT NULL,
  `CountryId` bigint(20) unsigned NOT NULL,
  `Name` varchar(256) NOT NULL,
  `CreatedBy` varchar(256) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `LastModifiedBy` varchar(256) NOT NULL,
  `LastModifiedDateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE loc_City
ADD CONSTRAINT `fk_loc_City_com_Agency` FOREIGN KEY (`AgencyId`) REFERENCES `com_Agency` (`Id`) ON DELETE CASCADE;
ALTER TABLE loc_City
ADD CONSTRAINT `fk_loc_City_loc_Country` FOREIGN KEY (`CountryId`) REFERENCES `loc_Country` (`Id`) ON DELETE CASCADE;