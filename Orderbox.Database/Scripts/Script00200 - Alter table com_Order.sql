ALTER TABLE com_Order
DROP COLUMN `Note`;

ALTER TABLE com_Order
CHANGE COLUMN `DeliveryAddress` `Description` TEXT NULL;

ALTER TABLE com_Order
ADD `BuyerEmailAddress` VARCHAR(256) NOT NULL AFTER `BuyerPhoneNumber`;