CREATE PROCEDURE FolderType_Create
(
    IN  Name    VARCHAR(255)
)
BEGIN

    INSERT INTO FolderType 
    	( Name, DateCreated )
    VALUES 
    	( Name, NOW() );
           
    SELECT last_insert_id();

END