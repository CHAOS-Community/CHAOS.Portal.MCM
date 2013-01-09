CREATE TABLE Object_Object_Join 
(
  Object1GUID           binary(16) NOT NULL,
  Object2GUID           binary(16) NOT NULL,
  ObjectRelationTypeID  int(10) unsigned NOT NULL,
  Sequence              int(11) DEFAULT NULL,
  DateCreated           datetime NOT NULL,

  PRIMARY KEY (Object1GUID,Object2GUID),
  KEY FK_ObjectRelationType_ID_Object_Object_Join_ObjectRelationTypeID (ObjectRelationTypeID),
  KEY FK_Object_GUID_Object_Object_Join_Object1GUID (Object1GUID),
  KEY FK_Object_GUID_Object_Object_Join_Object2GUID (Object2GUID),
  CONSTRAINT FK_ObjectRelationType_ID_Object_Object_Join_ObjectRelationTypeID FOREIGN KEY (ObjectRelationTypeID) REFERENCES ObjectRelationType (ID),
  CONSTRAINT FK_Object_GUID_Object_Object_Join_Object1GUID FOREIGN KEY (Object1GUID) REFERENCES Object (GUID),
  CONSTRAINT FK_Object_GUID_Object_Object_Join_Object2GUID FOREIGN KEY (Object2GUID) REFERENCES Object (GUID)
) 
ENGINE=InnoDB