ALTER TABLE com_Product
ADD `ValidPeriodStart` datetime NULL AFTER `IsAvailable`;
ALTER TABLE com_Product
ADD `ValidPeriodEnd` datetime NULL AFTER `ValidPeriodStart`;