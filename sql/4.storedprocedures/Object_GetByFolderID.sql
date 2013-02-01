CREATE PROCEDURE Object_GetByFolderID
(
    FolderID                INT UNSIGNED,
    IncludeMetadata         BOOLEAN,
    IncludeFiles                BOOLEAN,
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
        SELECT  Object.GUID
          FROM  Object
                INNER JOIN Object_Folder_Join ON Object_Folder_Join.ObjectGUID = Object.GUID
         WHERE  Object_Folder_Join.ObjectFolderTypeID = 1 AND
                ( FolderID IS NULL OR Object_Folder_Join.FolderID = FolderID )
         LIMIT  Offset, PageSize;
     
    SELECT  Object.*
      FROM  ObjectGUID_Table AS GT
            INNER JOIN Object ON Object.GUID = GT.GUID;
     
    IF( IncludeMetadata = 1 ) THEN
        SELECT M1.*
          FROM ObjectGUID_Table AS GT
               JOIN MCM.Metadata AS M1 ON GT.GUID = M1.ObjectGUID AND
                                                 M1.GUID = ( SELECT GUID
                                                        FROM MCM.Metadata AS M2
                                                       WHERE M2.ObjectGUID         = M1.ObjectGUID   AND
                                                             M2.LanguageCode       = M1.LanguageCode AND
                                                             M2.MetadataSchemaGUID = M1.MetadataSchemaGUID
                                                       ORDER BY M2.RevisionID DESC
                                                       LIMIT 1 );
    END IF;

    IF( IncludeFiles = 1 ) THEN
        SELECT  FileInfo.*
          FROM  ObjectGUID_Table AS GT
                INNER JOIN FileInfo ON FileInfo.ObjectGUID = GT.GUID;
    END IF;
                
    IF( IncludeObjectRelations = 1 ) THEN
        SELECT  DISTINCT Object_Object_Join.*
          FROM  ObjectGUID_Table AS GT
                INNER JOIN  Object_Object_Join ON
                            Object_Object_Join.Object1GUID = GT.GUID OR
                            Object_Object_Join.Object2GUID = GT.GUID;
    END IF;
    
    IF( IncludeFolders = 1 ) THEN
        SELECT  Object_Folder_Join.*
          FROM  ObjectGUID_Table AS GT
                INNER JOIN Object_Folder_Join ON Object_Folder_Join.ObjectGUID = GT.GUID;
    END IF;
    
    IF( IncludeAccessPoints = 1 ) THEN
        SELECT  AccessPoint_Object_Join.*
          FROM  ObjectGUID_Table AS GT
                INNER JOIN AccessPoint_Object_Join ON AccessPoint_Object_Join.ObjectGUID = GT.GUID;
    END IF;
END