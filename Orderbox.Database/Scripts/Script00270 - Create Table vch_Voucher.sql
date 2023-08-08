SET character_set_client = utf8mb4 ;
CREATE TABLE `vch_Voucher` (
  `Id` bigint(20) unsigned PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `OrderItemId` bigint(20) unsigned NOT NULL,
  `VoucherCode` varchar(64) NOT NULL,
  `Name` varchar(256) NOT NULL,
  `Description` text NULL,
  `TermAndCondition` text NULL,
  `RedeemMethod` varchar(32) NULL,
  `Status` varchar(32) NOT NULL,
  `ValidStartDate` datetime NULL,
  `ValidEndDate` datetime NULL,
  `CreatedBy` varchar(256) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `LastModifiedBy` varchar(256) NOT NULL,
  `LastModifiedDateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE vch_Voucher
ADD CONSTRAINT `fk_vch_Voucher_com_OrderItem` FOREIGN KEY (`OrderItemId`) REFERENCES `com_OrderItem` (`Id`) ON DELETE CASCADE;