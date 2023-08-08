ALTER TABLE `vch_Voucher` ADD COLUMN `RedeemDate` DATETIME NULL AFTER `Status`;

ALTER TABLE `vch_Voucher` ADD COLUMN `RedeemStoreId` bigint(20) unsigned NULL AFTER `RedeemDate`;

ALTER TABLE vch_Voucher
ADD CONSTRAINT `fk_vch_Voucher_loc_Store` FOREIGN KEY (`RedeemStoreId`) REFERENCES `loc_Store` (`Id`) ON DELETE SET NULL;