ALTER TABLE `Metadata` 
CHANGE COLUMN `MetadataXML` `MetadataXML` LONGTEXT CHARACTER SET 'utf8' COLLATE 'utf8_unicode_ci' NOT NULL ;

ALTER TABLE `Metadata` 
DROP PRIMARY KEY,
ADD PRIMARY KEY (`GUID`);

ALTER TABLE `Metadata` 
ADD INDEX `IX_GUID_RevisionID` (`GUID` ASC, `RevisionID` ASC);

