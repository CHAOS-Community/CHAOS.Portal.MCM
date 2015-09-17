CREATE PROCEDURE Object_Create
(
    Guid            BINARY(16),
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

	IF EXISTS(SELECT * FROM Object WHERE Object.GUID = Guid) THEN
		SELECT -1;
	ELSE
		START TRANSACTION;

			INSERT INTO Object
				( GUID, ObjectTypeID, DateCreated )
			VALUES
				( Guid, ObjectTypeID, NOW() );
               
			INSERT INTO Object_Folder_Join
				( ObjectGUID, FolderID, ObjectFolderTypeID, DateCreated )
			VALUES
				( Guid, FolderID, 1, NOW() );
         
		COMMIT;
         
		SELECT 1;
	END IF;

END