CREATE PROCEDURE ObjectFolderType_Create
(
    ID      INT UNSIGNED,
    Name	VARCHAR(255)
)
BEGIN

    INSERT INTO ObjectFolderType
    	( ID, Name ) 
	VALUES
		( ID, Name );
                          
    SELECT ROW_COUNT();
    
END