SET character_set_client = utf8mb4 ;
CREATE TABLE `com_Agent` (
  `Id` bigint(20) unsigned PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `AgencyId` bigint(20) unsigned NOT NULL,
  `UserId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Privilege` varchar(16) NOT NULL,
  `CreatedBy` varchar(256) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `LastModifiedBy` varchar(256) NOT NULL,
  `LastModifiedDateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;