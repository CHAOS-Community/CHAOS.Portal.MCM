CREATE PROCEDURE ObjectType_Delete
(
    ID      INT,
    Name    VARCHAR(255)
)
BEGIN

    IF( ID IS NULL AND Name IS NULL ) THEN
        SELECT -10;
    END IF;

    DELETE FROM ObjectType
    WHERE  
            ( ID   IS NULL OR ObjectType.ID = ID )
        AND ( Name IS NULL OR ObjectType.Name = Name );
     
     SELECT ROW_COUNT();

END