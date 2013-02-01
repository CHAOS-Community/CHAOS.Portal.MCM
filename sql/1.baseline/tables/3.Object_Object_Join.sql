CREATE TABLE Object_Object_Join 
(
	Object1Guid           binary(16) NOT NULL,
	Object2Guid           binary(16) NOT NULL,
	MetadataGuid			binary(16) DEFAULT NULL,
	ObjectRelationTypeID  int(10) unsigned NOT NULL,
	Sequence              int(11) DEFAULT NULL,
	DateCreated           datetime NOT NULL,

	PRIMARY KEY (Object1Guid,Object2Guid),
	KEY fk_ObjectRelationType_ID_Object_Object_Join_ObjectRelationTypeID (ObjectRelationTypeID),
	KEY fk_Object_Guid_Object_Object_Join_Object1Guid (Object1Guid),
	KEY fk_Object_Guid_Object_Object_Join_Object2Guid (Object2Guid),
	KEY fk_Object_Object_Join_Metadata_MetadataGuid_idx (MetadataGuid),
	CONSTRAINT fk_Object_Object_Join_Metadata_MetadataGuid FOREIGN KEY (MetadataGuid) REFERENCES Metadata (GUID),
	CONSTRAINT fk_ObjectRelationType_ID_Object_Object_Join_ObjectRelationTypeID FOREIGN KEY (ObjectRelationTypeID) REFERENCES ObjectRelationType (ID),
	CONSTRAINT fk_Object_GUID_Object_Object_Join_Object1Guid FOREIGN KEY (Object1Guid) REFERENCES Object (GUID),
	CONSTRAINT fk_Object_GUID_Object_Object_Join_Object2Guid FOREIGN KEY (Object2Guid) REFERENCES Object (GUID)
);