ALTER TABLE com_Order
ADD `PaymentStatus` VARCHAR(32) NOT NULL AFTER `PaymentDescription`;

ALTER TABLE com_Order
ADD `PaymentGatewayInvoiceUrl` TEXT NULL AFTER `PaymentStatus`;

ALTER TABLE com_Order
ADD `PaymentProof` TEXT NULL AFTER `PaymentGatewayInvoiceUrl`;

ALTER TABLE com_Order
ADD `PaymentChannel` VARCHAR(256) NULL AFTER `PaymentProof`;

ALTER TABLE com_Order
ADD `PaymentMethod` VARCHAR(256) NULL AFTER `PaymentChannel`;

ALTER TABLE com_Order
ADD `PaidAt` DATETIME NULL AFTER `PaymentMethod`;

UPDATE com_Order SET PaymentStatus = 'PAID';