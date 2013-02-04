CREATE PROCEDURE ObjectRelation_Set
(
    Object1Guid			        BINARY(16),
    Object2Guid			        BINARY(16),
    ObjectRelationTypeID		INT UNSIGNED,
    Sequence					INT
)
BEGIN

	INSERT INTO Object_Object_Join
		(Object1Guid,Object2Guid,ObjectRelationTypeID,Sequence,DateCreated)
	VALUES
		(Object1Guid,Object2Guid,ObjectRelationTypeID,Sequence,NOW());

    SELECT ROW_COUNT();

END