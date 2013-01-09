CREATE PROCEDURE ObjectRelation_Create
(
    Object1GUID			        BINARY(16),
    Object2GUID			        BINARY(16),
    ObjectRelationTypeID	  INT UNSIGNED,
    Sequence				        INT
)
BEGIN

    IF EXISTS( SELECT Object_Object_Join.Object1GUID 
                 FROM Object_Object_Join
                WHERE Object_Object_Join.Object1GUID = Object1GUID AND 
                      Object_Object_Join.Object2GUID = Object2GUID AND
                      Object_Object_Join.ObjectRelationTypeID = ObjectRelationTypeID  ) THEN
        SELECT -200;
    ELSE
        INSERT INTO Object_Object_Join
            ( Object1GUID, Object2GUID, ObjectRelationTypeID, Sequence, DateCreated )
        VALUES
            ( Object1GUID, Object2GUID, ObjectRelationTypeID, Sequence, NOW() );
    END IF;
    
    SELECT ROW_COUNT();

END