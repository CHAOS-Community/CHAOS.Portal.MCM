CREATE PROCEDURE ObjectRelation_Delete
(
    Object1GUID				BINARY(16),
    Object2GUID				BINARY(16),
    ObjectRelationTypeID	INT
)
BEGIN

    DELETE FROM Object_Object_Join
	WHERE  
			Object_Object_Join.Object1GUID = Object1GUID  
        AND	Object_Object_Join.Object2GUID = Object2GUID
        AND	Object_Object_Join.ObjectRelationTypeID = ObjectRelationTypeID;
            
    SELECT ROW_COUNT();

END