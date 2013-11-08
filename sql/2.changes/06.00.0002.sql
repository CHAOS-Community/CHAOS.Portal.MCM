ALTER TABLE `Object_Metadata_Join` DROP FOREIGN KEY `fk_Object_Metadata_Join_Metadata_MetadataGuid` ;

ALTER TABLE `Object_Metadata_Join` 

  ADD CONSTRAINT `fk_Object_Metadata_Join_Metadata_MetadataGuid`

  FOREIGN KEY (`MetadataGuid` )

  REFERENCES `Metadata` (`GUID` )

  ON DELETE CASCADE

  ON UPDATE RESTRICT;

  INSERT INTO `Version`(`Major`,`Minor`,`Release`,`ScriptName`,`DateCreated`)
VALUES ('06','00','0002','06.00.0002.sql',NOW());
