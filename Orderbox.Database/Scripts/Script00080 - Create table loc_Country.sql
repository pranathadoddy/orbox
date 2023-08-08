SET character_set_client = utf8mb4 ;
CREATE TABLE `loc_Country` (
  `Id` bigint(20) unsigned PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `AgencyId` bigint(20) unsigned NOT NULL,
  `Name` varchar(256) NOT NULL,
  `CreatedBy` varchar(256) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `LastModifiedBy` varchar(256) NOT NULL,
  `LastModifiedDateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE loc_Country
ADD CONSTRAINT `fk_loc_Country_com_Agency` FOREIGN KEY (`AgencyId`) REFERENCES `com_Agency` (`Id`) ON DELETE CASCADE;