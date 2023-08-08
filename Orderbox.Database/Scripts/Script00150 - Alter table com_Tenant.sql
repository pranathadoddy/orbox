ALTER TABLE com_Tenant
ADD `AllowToAccessCategory` bit(1) NOT NULL AFTER `EnableShop`;
ALTER TABLE com_Tenant
ADD `AllowToAccessProduct` bit(1) NOT NULL AFTER `AllowToAccessCategory`;
