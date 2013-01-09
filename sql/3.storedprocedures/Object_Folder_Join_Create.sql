CREATE PROCEDURE Object_Folder_Join_Create
(
    ObjectGUID          BINARY(16),
    FolderID            INT,
    ObjectFolderTypeID  INT
)
BEGIN

    IF( ObjectFolderTypeID = 1 ) THEN
        SELECT -100;
    END IF;

    INSERT INTO Object_Folder_Join
    	( ObjectGUID, FolderID, ObjectFolderTypeID, DateCreated )
	VALUES
		( ObjectGUID, FolderID, ObjectFolderTypeID, NOW() );
 
    SELECT ROW_COUNT();

END