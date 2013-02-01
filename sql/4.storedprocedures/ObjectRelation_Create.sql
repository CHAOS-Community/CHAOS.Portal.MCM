CREATE PROCEDURE ObjectRelation_Create
(
    Object1Guid			        BINARY(16),
    Object2Guid			        BINARY(16),
	MetadataGuid				BINARY(16),
    ObjectRelationTypeID		INT UNSIGNED,
    Sequence					INT
)
BEGIN

	INSERT INTO Object_Object_Join
		(Object1Guid,Object2Guid,MetadataGuid,ObjectRelationTypeID,Sequence,DateCreated)
	VALUES
		(Object1Guid,Object2Guid,MetadataGuid,ObjectRelationTypeID,Sequence,NOW());


    SELECT ROW_COUNT();

END