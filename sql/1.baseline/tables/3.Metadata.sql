CREATE TABLE Metadata 
(
  GUID                binary(16) NOT NULL,
  LanguageCode        varchar(10),
  MetadataSchemaGUID  binary(16) NOT NULL,
  RevisionID          int(10) unsigned NOT NULL DEFAULT '0',
  MetadataXML         MEDIUMTEXT NOT NULL,
  DateCreated         datetime NOT NULL,
  EditingUserGUID     binary(16) NOT NULL,

  PRIMARY KEY (GUID,DateCreated),
  KEY FK_Language_LanguageCode_Metadata_LanguageCode (LanguageCode),
  KEY FK_MetadataSchema_GUID_Metadata_MetadataSchemaGUID (MetadataSchemaGUID),
  CONSTRAINT FK_MetadataSchema_GUID_Metadata_MetadataSchemaGUID FOREIGN KEY (MetadataSchemaGUID) REFERENCES MetadataSchema (GUID)
) 
ENGINE=InnoDB