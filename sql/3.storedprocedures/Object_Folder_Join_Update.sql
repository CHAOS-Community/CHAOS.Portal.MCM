CREATE PROCEDURE Object_Folder_Join_Update
(
    ObjectGuid      BINARY(16),
    FolderID        INT,
    NewFolderID     INT
)
BEGIN

    UPDATE  
    	Object_Folder_Join
	SET  
		Object_Folder_Join.FolderID = NewFolderID
	WHERE  
			Object_Folder_Join.ObjectGUID = ObjectGuid
        AND Object_Folder_Join.FolderID   = FolderID;
            
    SELECT ROW_COUNT();

END