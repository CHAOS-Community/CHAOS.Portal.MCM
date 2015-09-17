CREATE PROCEDURE MetadataSchema_Create
(
    Guid        BINARY(16), 
    Name        VARCHAR(255), 
    SchemaXml   TEXT,
    UserGuid    BINARY(16)
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
            ( Guid, Name, SchemaXml, NOW() );
                            
        INSERT INTO MetadataSchema_User_Join
            ( MetadataSchemaGUID, UserGUID, Permission, DateCreated ) 
        VALUES
            ( Guid, UserGuid, 4294967295, NOW() );
         
    COMMIT;
         
    SELECT 1;

END