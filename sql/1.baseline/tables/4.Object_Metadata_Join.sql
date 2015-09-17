CREATE TABLE Object_Metadata_Join
(
	ObjectGuid		BINARY(16) NOT NULL,
	MetadataGuid	BINARY(16) NOT NULL,

	PRIMARY KEY (ObjectGuid, MetadataGuid),
	KEY fk_Object_Metadata_Join_Metadata_MetadataGuid_idx (MetadataGuid),
	KEY fk_Object_Metadata_Join_Object_ObjectGuid_idx (ObjectGuid),
	CONSTRAINT fk_Object_Metadata_Join_Metadata_MetadataGuid FOREIGN KEY (MetadataGuid) REFERENCES Metadata (GUID),
	CONSTRAINT fk_Object_Metadata_Join_Object_ObjectGuid FOREIGN KEY (ObjectGuid) REFERENCES Object (GUID)
);