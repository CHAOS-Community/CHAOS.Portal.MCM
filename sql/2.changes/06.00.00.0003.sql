ALTER TABLE `Metadata` CHANGE COLUMN `MetadataXML` `MetadataXML` MEDIUMTEXT NOT NULL  ;

  INSERT INTO `Version`(`Major`,`Minor`,`Release`,`ScriptName`,`DateCreated`)
VALUES ('06','00','0003','06.00.0003.sql',NOW());
