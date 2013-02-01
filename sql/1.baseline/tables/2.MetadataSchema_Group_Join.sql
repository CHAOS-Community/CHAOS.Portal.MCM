CREATE TABLE MetadataSchema_Group_Join 
(
  MetadataSchemaGUID 	binary(16) NOT NULL,
  GroupGUID 			binary(16) NOT NULL,
  Permission 			int(10) unsigned NOT NULL,
  DateCreated 			datetime NOT NULL,

  PRIMARY KEY (MetadataSchemaGUID,GroupGUID),
  KEY FK_MS_GUID_MS_Group_Join_MSGUID (MetadataSchemaGUID),
  CONSTRAINT FK_MS_GUID_MS_Group_Join_MSGUID FOREIGN KEY (MetadataSchemaGUID) REFERENCES MetadataSchema (GUID)
) 
ENGINE=InnoDB