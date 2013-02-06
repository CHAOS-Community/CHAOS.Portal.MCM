﻿INSERT INTO ObjectType(ID,Name)VALUES(1,'Test Type');
INSERT INTO ObjectRelationType(ID,Name)VALUES(1,'test relation type');

INSERT INTO Object(GUID,ObjectTypeID,DateCreated)VALUES(unhex('00000000000000000000000000000001'),1,'1990-10-01 23:59:59');
INSERT INTO Object(GUID,ObjectTypeID,DateCreated)VALUES(unhex('00000000000000000000000000000002'),1,'1990-10-01 23:59:59');
INSERT INTO Object(GUID,ObjectTypeID,DateCreated)VALUES(unhex('00000000000000000000000000000003'),1,'1991-10-01 23:59:59');

INSERT INTO MetadataSchema(GUID,Name,SchemaXML,DateCreated)VALUES
(unhex('00000000000000000000000000000100'),'test schema','<xml/>','1990-10-01 23:59:59');

INSERT INTO Metadata(GUID,LanguageCode,MetadataSchemaGUID,RevisionID,MetadataXML,DateCreated,EditingUserGUID)VALUES
(unhex('00000000000000000000000000000010'),'en',unhex('00000000000000000000000000000100'),0,'<xml>test xml</xml>','1990-10-01 23:59:59',unhex('00000000000000000000000000000000'));

INSERT INTO Metadata(GUID,LanguageCode,MetadataSchemaGUID,RevisionID,MetadataXML,DateCreated,EditingUserGUID)VALUES
(unhex('00000000000000000000000000000050'),'en',unhex('00000000000000000000000000000100'),0,'<xml>test xml</xml>','1990-10-01 23:59:59',unhex('00000000000000000000000000000000'));

INSERT INTO Object_Metadata_Join (ObjectGuid,MetadataGuid) VALUES (unhex('00000000000000000000000000000002'),unhex('00000000000000000000000000000050'));

INSERT INTO Object_Object_Join(Object1Guid,Object2Guid,MetadataGuid,ObjectRelationTypeID,Sequence,DateCreated)VALUES
(unhex('00000000000000000000000000000001'),unhex('00000000000000000000000000000002'),unhex('00000000000000000000000000000010'),1,null,'1990-10-01 23:59:59');