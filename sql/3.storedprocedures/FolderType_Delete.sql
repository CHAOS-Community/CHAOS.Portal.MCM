CREATE PROCEDURE FolderType_Delete
(
    IN ID   INT,
)
BEGIN

    DELETE FROM  
        FolderType
     WHERE	
        FolderType.ID = ID;
     
    SELECT ROW_COUNT();

END