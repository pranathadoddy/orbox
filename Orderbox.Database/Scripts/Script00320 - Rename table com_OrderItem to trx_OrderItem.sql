ALTER TABLE `com_OrderItem` 
DROP FOREIGN KEY `fk_com_OrderItem_com_Tenant1`,
DROP FOREIGN KEY `fk_com_OrderItem_com_Order1`;

ALTER TABLE `com_OrderItem` 
DROP INDEX `fk_com_OrderItem_com_Tenant1_idx` ,
DROP INDEX `fk_com_OrderItem_com_Order1` ;

RENAME TABLE `com_OrderItem` TO `trx_OrderItem`;

ALTER TABLE `trx_OrderItem`
ADD CONSTRAINT `fk_trx_OrderItem_com_Tenant` FOREIGN KEY (`TenantId`) REFERENCES `com_Tenant` (`Id`) ON DELETE CASCADE;

ALTER TABLE `trx_OrderItem`
ADD CONSTRAINT `fk_trx_OrderItem_trx_Order` FOREIGN KEY (`OrderId`) REFERENCES `trx_Order` (`Id`) ON DELETE CASCADE;