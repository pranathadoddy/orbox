ALTER TABLE `vch_Voucher` 
DROP FOREIGN KEY `fk_vch_Voucher_loc_Store`;

ALTER TABLE `vch_Voucher` 
DROP INDEX `fk_vch_Voucher_loc_Store`;