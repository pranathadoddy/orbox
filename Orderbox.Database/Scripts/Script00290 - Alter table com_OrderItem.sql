ALTER TABLE com_OrderItem
ADD `ProductType` VARCHAR(256) NULL AFTER `ProductUnit`;

ALTER TABLE com_OrderItem
ADD `ProductDescription` TEXT NULL AFTER `ProductUnit`;

ALTER TABLE com_OrderItem
ADD `ProductTermAndCondition` TEXT NULL AFTER `ProductUnit`;

ALTER TABLE com_OrderItem
ADD `ProductRedeemMethod` VARCHAR(32) NULL AFTER `ProductUnit`;

ALTER TABLE com_OrderItem
ADD `ValidStartDate` DATETIME NULL AFTER `ProductUnit`;

ALTER TABLE com_OrderItem
ADD `ValidEndDate` DATETIME NULL AFTER `ProductUnit`;
