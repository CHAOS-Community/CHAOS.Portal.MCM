ALTER TABLE `Metadata` 
CHANGE COLUMN `MetadataXML` `MetadataXML` LONGTEXT CHARACTER SET 'utf8' COLLATE 'utf8_unicode_ci' NOT NULL ;

  INSERT INTO `Version`(`Major`,`Minor`,`Release`,`ScriptName`,`DateCreated`)
VALUES ('06','00','0004','06.00.0004.sql',NOW());
