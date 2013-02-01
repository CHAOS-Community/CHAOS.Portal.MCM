CREATE PROCEDURE Object_Folder_Join_Delete
(
    ObjectGUID  BINARY(16),
    FolderID    INT UNSIGNED
)
BEGIN

    DELETE FROM Object_Folder_Join
	WHERE  
			Object_Folder_Join.ObjectGUID = ObjectGUID
        AND Object_Folder_Join.FolderID   = FolderID;
            
    SELECT ROW_COUNT();

END