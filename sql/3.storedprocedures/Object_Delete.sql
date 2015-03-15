CREATE PROCEDURE Object_Delete
(
    Guid	BINARY(16)
)
BEGIN

    DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    START TRANSACTION;

		DELETE
			Metadata, Object_Metadata_Join
		FROM
			Metadata
			INNER JOIN Object_Metadata_Join ON Metadata.GUID = Object_Metadata_Join.MetadataGuid
		WHERE
			Object_Metadata_Join.ObjectGuid = Guid;

        DELETE FROM Object_Folder_Join
        WHERE
            Object_Folder_Join.ObjectGUID = Guid;
                                 
        DELETE FROM AccessPoint_Object_Join
         WHERE  
            AccessPoint_Object_Join.ObjectGUID = Guid;
        
        DELETE FROM File
        WHERE  
            File.ObjectGUID = Guid;
        
		DELETE
			Metadata, Object_Object_Join
		FROM
			Metadata
			INNER JOIN Object_Object_Join ON Metadata.GUID = Object_Object_Join.MetadataGuid
		WHERE
				Object_Object_Join.Object1GUID = Guid
            OR  Object_Object_Join.Object2GUID = Guid;
        
        DELETE FROM Object
        WHERE  
            Object.GUID = Guid;
              
    COMMIT;
    
    SELECT 1;
END