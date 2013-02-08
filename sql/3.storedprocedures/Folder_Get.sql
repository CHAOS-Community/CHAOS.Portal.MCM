CREATE PROCEDURE Folder_Get
(
  IN  ID                  INT,
  IN  ObjectGUID          BINARY(16)
)
BEGIN

    IF( ObjectGUID IS NULL ) THEN
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
          Object_Folder_Join.ObjectGUID = ObjectGUID
          AND ( ID IS NULL OR Folder.ID = ID );
    END IF;

END