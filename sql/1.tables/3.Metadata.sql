CREATE TABLE Metadata 
(
  GUID                binary(16) NOT NULL,
  ObjectGUID          binary(16) NOT NULL,
  LanguageCode        varchar(10),
  MetadataSchemaGUID  binary(16) NOT NULL,
  RevisionID          int(10) unsigned NOT NULL DEFAULT '0',
  MetadataXML         text NOT NULL,
  DateCreated         datetime NOT NULL,
  EditingUserGUID     binary(16) NOT NULL,

  PRIMARY KEY (GUID,DateCreated),
  KEY FK_Language_LanguageCode_Metadata_LanguageCode (LanguageCode),
  KEY FK_MetadataSchema_GUID_Metadata_MetadataSchemaGUID (MetadataSchemaGUID),
  KEY FK_Object_GUID_Metadata_ObjectGUID (ObjectGUID),
  CONSTRAINT FK_MetadataSchema_GUID_Metadata_MetadataSchemaGUID FOREIGN KEY (MetadataSchemaGUID) REFERENCES MetadataSchema (GUID),
  CONSTRAINT FK_Object_GUID_Metadata_ObjectGUID FOREIGN KEY (ObjectGUID) REFERENCES Object (GUID)
) 
ENGINE=InnoDB