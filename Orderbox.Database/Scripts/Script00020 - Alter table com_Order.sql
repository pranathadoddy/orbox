ALTER TABLE com_Order
ADD `CustomerId` bigint(20) unsigned NULL AFTER `TenantId`;
ALTER TABLE com_Order
ADD CONSTRAINT `fk_com_Order_com_Customer` FOREIGN KEY (`CustomerId`) REFERENCES `com_Customer` (`Id`) ON DELETE CASCADE;