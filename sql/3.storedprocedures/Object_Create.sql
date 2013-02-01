CREATE PROCEDURE Object_Create
(
    GUID            BINARY(16),
    ObjectTypeID    INT UNSIGNED,
    FolderID        INT UNSIGNED
)
BEGIN
    
    DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    START TRANSACTION;

        INSERT INTO Object
            ( GUID, ObjectTypeID, DateCreated )
        VALUES
            ( GUID, ObjectTypeID, NOW() );
               
        INSERT INTO Object_Folder_Join
            ( ObjectGUID, FolderID, ObjectFolderTypeID, DateCreated )
        VALUES
            ( GUID, FolderID, 1, NOW() );
         
    COMMIT;
         
    SELECT 1;

END