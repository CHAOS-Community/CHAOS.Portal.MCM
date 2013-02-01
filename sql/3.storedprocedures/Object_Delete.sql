CREATE PROCEDURE Object_Delete
(
    GUID        BINARY(16)
)
BEGIN

    DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    START TRANSACTION;

        DELETE FROM Object_Folder_Join
        WHERE
            Object_Folder_Join.ObjectGUID = GUID;
                
        DELETE FROM Metadata
        WHERE
            Metadata.ObjectGUID = GUID;
                 
        DELETE FROM AccessPoint_Object_Join
         WHERE  
            AccessPoint_Object_Join.ObjectGUID = GUID;
        
        DELETE FROM File
        WHERE  
            File.ObjectGUID = GUID;
         
        DELETE FROM Object_Object_Join
        WHERE 
                Object_Object_Join.Object1GUID = GUID
            OR  Object_Object_Join.Object2GUID = GUID;
        
        DELETE FROM Object
        WHERE  
            Object.GUID = GUID;
              
    COMMIT;
    
    SELECT 1;
END