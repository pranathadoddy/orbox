ALTER TABLE `com_Order` 
DROP FOREIGN KEY `fk_com_Order_com_Tenant1`,
DROP FOREIGN KEY `fk_com_Order_com_Customer`;

ALTER TABLE `com_Order` 
DROP INDEX `fk_com_Order_com_Customer`,
DROP INDEX `fk_com_Order_com_Tenant1` ;

RENAME TABLE `com_Order` TO `trx_Order`;

ALTER TABLE `trx_Order`
ADD CONSTRAINT `fk_trx_Order_com_Tenant` FOREIGN KEY (`TenantId`) REFERENCES `com_Tenant` (`Id`) ON DELETE CASCADE;

ALTER TABLE `trx_Order`
ADD CONSTRAINT `fk_trx_Order_com_Customer` FOREIGN KEY (`CustomerId`) REFERENCES `com_Customer` (`Id`) ON DELETE SET NULL;