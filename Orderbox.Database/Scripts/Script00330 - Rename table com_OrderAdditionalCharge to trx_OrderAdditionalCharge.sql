ALTER TABLE `com_OrderAdditionalCharge` 
DROP FOREIGN KEY `fk_com_OrderAdditionalCharge_com_Tenant1`,
DROP FOREIGN KEY `fk_com_OrderAdditionalCharge_com_Order1`;

ALTER TABLE `com_OrderAdditionalCharge` 
DROP INDEX `fk_com_OrderAdditionalCharge_com_Tenant1_idx` ,
DROP INDEX `fk_com_OrderAdditionalCharge_com_Order1` ;

RENAME TABLE `com_OrderAdditionalCharge` TO `trx_OrderAdditionalCharge`;

ALTER TABLE `trx_OrderAdditionalCharge`
ADD CONSTRAINT `fk_trx_OrderAdditionalCharge_com_Tenant` FOREIGN KEY (`TenantId`) REFERENCES `com_Tenant` (`Id`) ON DELETE CASCADE;

ALTER TABLE `trx_OrderAdditionalCharge`
ADD CONSTRAINT `fk_trx_OrderAdditionalCharge_trx_Order` FOREIGN KEY (`OrderId`) REFERENCES `trx_Order` (`Id`) ON DELETE CASCADE;