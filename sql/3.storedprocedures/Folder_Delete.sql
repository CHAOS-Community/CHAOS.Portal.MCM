CREATE PROCEDURE Folder_Delete
(
    ID  INT
)
BEGIN

    DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    IF EXISTS( SELECT * FROM Folder WHERE Folder.ParentID = ID ) THEN
        SELECT -50;
    END IF;
        
    IF EXISTS( SELECT * FROM Object_Folder_Join WHERE FolderID = ID ) THEN
        SELECT -50;
    END IF;
        
    START TRANSACTION;

    DELETE 
      FROM	Folder_Group_Join
     WHERE	FolderID = ID;

    DELETE 
      FROM	Folder_User_Join
     WHERE	FolderID = ID;
     
    DELETE 
      FROM	Folder
     WHERE	Folder.ID = ID;

    COMMIT;
        
    SELECT 1;

END