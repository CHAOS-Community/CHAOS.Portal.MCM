CREATE PROCEDURE MetadataSchema_Delete
(
    Guid	BINARY(16)
)
BEGIN
    DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    START TRANSACTION;

        DELETE FROM MetadataSchema_User_Join
        WHERE  
            MetadataSchemaGUID = Guid;
         
        DELETE FROM MetadataSchema_Group_Join
        WHERE
            MetadataSchemaGUID = Guid;
          
        DELETE FROM MetadataSchema
        WHERE 
            MetadataSchema.GUID = Guid;
         
    COMMIT;
         
    SELECT 1;

END