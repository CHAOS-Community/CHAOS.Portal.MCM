CREATE TABLE Object 
(
  GUID 			binary(16) NOT NULL,
  ObjectTypeID 	int(10) unsigned NOT NULL,
  DateCreated	datetime NOT NULL,

  PRIMARY KEY (GUID),
  UNIQUE KEY GUID_UNIQUE (GUID),
  KEY FK_ObjectType_ID_Object_ObjectTypeID (ObjectTypeID),
  CONSTRAINT FK_ObjectType_ID_Object_ObjectTypeID FOREIGN KEY (ObjectTypeID) REFERENCES ObjectType (ID)
) 
ENGINE=InnoDB