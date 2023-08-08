ALTER TABLE `com_Product` ADD COLUMN `Type` VARCHAR(256) NOT NULL AFTER `CategoryId`;

UPDATE com_Product SET `Type` = 'PRODUCT';