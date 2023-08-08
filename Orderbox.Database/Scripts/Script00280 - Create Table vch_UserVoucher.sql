SET character_set_client = utf8mb4 ;
CREATE TABLE `vch_CustomerVoucher` (
  `Id` bigint(20) unsigned PRIMARY KEY NOT NULL AUTO_INCREMENT,
  `CustomerId` bigint(20) unsigned NOT NULL,
  `VoucherId` bigint(20) unsigned NOT NULL,
  `CreatedBy` varchar(256) NOT NULL,
  `CreatedDateTime` datetime NOT NULL,
  `LastModifiedBy` varchar(256) NOT NULL,
  `LastModifiedDateTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

ALTER TABLE vch_CustomerVoucher
ADD CONSTRAINT `fk_vch_CustomerVoucher_com_Customer` FOREIGN KEY (`CustomerId`) REFERENCES `com_Customer` (`Id`) ON DELETE CASCADE;

ALTER TABLE vch_CustomerVoucher
ADD CONSTRAINT `fk_vch_CustomerVoucher_vch_Voucher` FOREIGN KEY (`VoucherId`) REFERENCES `vch_Voucher` (`Id`) ON DELETE CASCADE;