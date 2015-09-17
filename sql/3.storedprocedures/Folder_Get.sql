CREATE PROCEDURE Folder_Get
(
	ID			INT,
	ObjectGuid	BINARY(16)
)
BEGIN

    IF( ObjectGuid IS NULL ) THEN
        SELECT  
          *
        FROM
          Folder
        WHERE
          ( ID IS NULL OR Folder.ID = ID );
    ELSE
        SELECT
          Folder.*
        FROM
          Object_Folder_Join
          INNER JOIN Folder ON Object_Folder_Join.FolderID = Folder.ID
        WHERE  
          Object_Folder_Join.ObjectGUID = ObjectGuid
          AND ( ID IS NULL OR Folder.ID = ID );
    END IF;

END