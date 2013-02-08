CREATE TABLE MetadataSchema_User_Join 
(
  MetadataSchemaGUID 	binary(16) NOT NULL,
  UserGUID 				binary(16) NOT NULL,
  Permission 			int(10) unsigned NOT NULL,
  DateCreated 			datetime NOT NULL,

  PRIMARY KEY (MetadataSchemaGUID,UserGUID),
  KEY FK_MS_GUID_MS_User_Join_MSGUID (MetadataSchemaGUID),
  CONSTRAINT FK_MS_GUID_MS_User_Join_MetadataSchemaGUID FOREIGN KEY (MetadataSchemaGUID) REFERENCES MetadataSchema (GUID)
) 
ENGINE=InnoDB