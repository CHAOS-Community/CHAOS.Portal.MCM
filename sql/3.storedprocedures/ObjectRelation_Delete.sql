CREATE PROCEDURE ObjectRelation_Delete
(
    Object1Guid				BINARY(16),
    Object2Guid				BINARY(16),
    ObjectRelationTypeID	INT
)
BEGIN

    DELETE FROM Object_Object_Join
	WHERE  
			Object_Object_Join.Object1GUID = Object1Guid  
        AND	Object_Object_Join.Object2GUID = Object2Guid
        AND	Object_Object_Join.ObjectRelationTypeID = ObjectRelationTypeID;
            
    SELECT ROW_COUNT();

END