ALTER TABLE com_Payment
ADD `ApiKey` text NULL AFTER `Description`;
ALTER TABLE com_Payment
ADD `WebhookValidationSecret` text NULL AFTER `ApiKey`;
ALTER TABLE com_Payment
ADD `ActiveDurationInMinutes` int NULL AFTER `WebhookValidationSecret`;