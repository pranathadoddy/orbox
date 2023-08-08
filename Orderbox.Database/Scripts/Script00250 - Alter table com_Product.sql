ALTER TABLE `com_Product` ADD COLUMN `Commission` DOUBLE NULL AFTER `ValidPeriodEnd`;

ALTER TABLE `com_Product` ADD COLUMN `TermAndCondition` TEXT NULL AFTER `Commission`;

ALTER TABLE `com_Product` ADD COLUMN `RedeemMethod` VARCHAR(32) NULL AFTER `TermAndCondition`;