ALTER TABLE com_Tenant
ADD `AllowToAccessProfile` bit(1) NOT NULL AFTER `AllowToAccessProduct`;

ALTER TABLE com_Tenant
ADD `AllowToAccessCheckoutSetting` bit(1) NOT NULL AFTER `AllowToAccessProfile`;