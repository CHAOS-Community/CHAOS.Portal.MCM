CREATE PROCEDURE FolderType_Update
(
    IN  ID      INT,
    IN  Name    VARCHAR(255),
)
BEGIN

    UPDATE
        FolderType
    SET	
        FolderType.Name = @Name
    WHERE	
        FolderType.ID = ID;
     
    SELECT ROW_COUNT();

END