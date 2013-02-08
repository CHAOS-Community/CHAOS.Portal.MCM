CREATE  TABLE Object_Metadata_Join(	ObjectGuid		BINARY(16) NOT NULL,	MetadataGuid	BINARY(16) NOT NULL,
	PRIMARY KEY (ObjectGuid, MetadataGuid) 
	KEY fk_Object_Metadata_Join_Metadata_MetadataGuid_idx (MetadataGuid),
	KEY fk_Object_Metadata_Join_Object_ObjectGuid_idx (ObjectGuid),
	CONSTRAINT fk_Object_Metadata_Join_Metadata_MetadataGuid FOREIGN KEY (MetadataGuid) REFERENCES Metadata (GUID),
	CONSTRAINT fk_Object_Metadata_Join_Object_ObjectGuid FOREIGN KEY (ObjectGuid) REFERENCES Object (GUID)
);

ALTER TABLE `Object_Object_Join` ADD COLUMN `MetadataGuid` BINARY(16) NULL  AFTER `Object2GUID` , 
  ADD CONSTRAINT `fk_Object_Object_Join_Metadata_MetadataGuid`
  FOREIGN KEY (`MetadataGuid` )
  REFERENCES `test_mcm`.`Metadata` (`GUID` )
  ON DELETE NO ACTION
  ON UPDATE NO ACTION
, ADD INDEX `fk_Object_Object_Join_Metadata_MetadataGuid_idx` (`MetadataGuid` ASC) ;

ALTER TABLE `test_mcm`.`Object_Object_Join` DROP FOREIGN KEY `FK_Object_GUID_Object_Object_Join_Object1GUID` , DROP FOREIGN KEY `FK_Object_GUID_Object_Object_Join_Object2GUID` ;
ALTER TABLE `test_mcm`.`Object_Object_Join` CHANGE COLUMN `Object1GUID` `Object1Guid` BINARY(16) NOT NULL  , CHANGE COLUMN `Object2GUID` `Object2Guid` BINARY(16) NOT NULL  , 
  ADD CONSTRAINT `FK_Object_GUID_Object_Object_Join_Object1GUID`
  FOREIGN KEY (`Object1Guid` )
  REFERENCES `test_mcm`.`Object` (`GUID` ), 
  ADD CONSTRAINT `FK_Object_GUID_Object_Object_Join_Object2GUID`
  FOREIGN KEY (`Object2Guid` )
  REFERENCES `test_mcm`.`Object` (`GUID` )
, DROP INDEX `FK_ObjectRelationType_ID_Object_Object_Join_ObjectRelationTypeID` 
, ADD INDEX `fk_ObjectRelationType_ID_Object_Object_Join_ObjectRelationTypeID` (`ObjectRelationTypeID` ASC) 
, DROP INDEX `FK_Object_GUID_Object_Object_Join_Object1GUID` 
, ADD INDEX `fk_Object_GUID_Object_Object_Join_Object1Guid` (`Object1Guid` ASC) 
, DROP INDEX `FK_Object_GUID_Object_Object_Join_Object2GUID` 
, ADD INDEX `fk_Object_GUID_Object_Object_Join_Object2Guid` (`Object2Guid` ASC) ;

ALTER TABLE `test_mcm`.`Metadata` DROP FOREIGN KEY `FK_Object_GUID_Metadata_ObjectGUID` ;
ALTER TABLE `test_mcm`.`Metadata` DROP COLUMN `ObjectGUID` , CHANGE COLUMN `LanguageCode` `LanguageCode` VARCHAR(10) NULL  
, DROP INDEX `FK_Object_GUID_Metadata_ObjectGUID` ;
