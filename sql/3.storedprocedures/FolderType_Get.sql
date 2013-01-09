CREATE PROCEDURE FolderType_Get
(
    IN  ID		INT,
    IN  Name	VARCHAR(255)
)
BEGIN

    SELECT	
    	*
	FROM	
		FolderType
	WHERE	
		( ID   IS NULL OR FolderType.ID   = ID ) AND 
        ( Name IS NULL OR FolderType.Name = Name );

END