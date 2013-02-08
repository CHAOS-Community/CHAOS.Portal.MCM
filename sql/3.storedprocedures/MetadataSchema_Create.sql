CREATE PROCEDURE MetadataSchema_Create
(
    GUID        BINARY(16), 
    Name        VARCHAR(255), 
    SchemaXML   TEXT,
    UserGUID    BINARY(16)
)
BEGIN

DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    START TRANSACTION;

        INSERT INTO MetadataSchema
            ( GUID, Name, SchemaXML, DateCreated ) 
        VALUES
            ( GUID, Name, SchemaXML, NOW() );
                            
        INSERT INTO MetadataSchema_User_Join
            ( MetadataSchemaGUID, UserGUID, Permission, DateCreated ) 
        VALUES
            ( GUID, UserGUID, 4294967295, NOW() );
         
    COMMIT;
         
    SELECT 1;

END