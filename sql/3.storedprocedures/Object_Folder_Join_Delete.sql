CREATE PROCEDURE Object_Folder_Join_Delete
(
    ObjectGuid  BINARY(16),
    FolderID    INT UNSIGNED
)
BEGIN

    DELETE FROM Object_Folder_Join
	WHERE  
			Object_Folder_Join.ObjectGUID = ObjectGuid
        AND Object_Folder_Join.FolderID   = FolderID;
            
    SELECT ROW_COUNT();

END