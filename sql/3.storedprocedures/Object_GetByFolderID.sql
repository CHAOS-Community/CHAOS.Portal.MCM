CREATE PROCEDURE Object_GetByFolderID
(
    FolderID                INT UNSIGNED,
    IncludeMetadata         BOOLEAN,
    IncludeFiles			BOOLEAN,
    IncludeObjectRelations  BOOLEAN,
    IncludeFolders          BOOLEAN,
    IncludeAccessPoints     BOOLEAN,
    PageIndex               INT UNSIGNED,
    PageSize                INT UNSIGNED
)
BEGIN

    DECLARE Offset INT UNSIGNED;
    
    CREATE TEMPORARY TABLE IF NOT EXISTS ObjectGUID_Table 
    (
        GUID    BINARY(16) NOT NULL
    );

    DELETE FROM ObjectGUID_Table;
    
    SET Offset = PageIndex * PageSize;
    
    INSERT INTO ObjectGUID_Table
		SELECT  
			OFJ.ObjectGUID
		FROM  
			Object_Folder_Join AS OFJ
		WHERE  
				OFJ.ObjectFolderTypeID = 1
			AND	(FolderID IS NULL OR OFJ.FolderID = FolderID)
		LIMIT  
			Offset, PageSize;
     
    SELECT  Object.*
      FROM  ObjectGUID_Table AS GT
            INNER JOIN Object ON Object.GUID = GT.GUID;
     
	SELECT 
		OM.*
    FROM
		ObjectGUID_Table AS GT
		INNER JOIN ObjectMetadata AS OM ON GT.GUID = OM.ObjectGuid
    WHERE 
		IncludeMetadata = 1;
        
	SELECT  
		FI.*
	FROM  
		ObjectGUID_Table AS GT
		INNER JOIN FileInfo AS FI ON FI.ObjectGUID = GT.GUID
	WHERE
		IncludeFiles = 1;
                
    SELECT 
		DISTINCT 
		ORI.* 
	FROM 
		ObjectGUID_Table AS GT
		INNER JOIN ObjectRelationInfo AS ORI ON GT.GUID = ORI.Object1Guid OR GT.GUID = ORI.Object2Guid
	WHERE 
		IncludeObjectRelations = 1;
    
	SELECT  
		OF.*
	FROM  
		ObjectGUID_Table AS GT
		INNER JOIN ObjectFolder AS OF ON OF.ObjectGUID = GT.GUID
	WHERE
		IncludeFolders = 1;
    
	SELECT  
		AOJ.*
	FROM  
		ObjectGUID_Table AS GT
		INNER JOIN AccessPoint_Object_Join AS AOJ ON AOJ.ObjectGUID = GT.GUID
	WHERE
		IncludeAccessPoints = 1;
END