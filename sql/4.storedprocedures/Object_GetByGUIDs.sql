CREATE PROCEDURE Object_GetByGUIDs
(
    GUIDs                   VARCHAR(21845),
    IncludeMetadata         BOOLEAN,
    IncludeFiles            BOOLEAN,
    IncludeObjectRelations  BOOLEAN,
    IncludeFolders          BOOLEAN,
    IncludeAccessPoints     BOOLEAN
)
BEGIN

    SET @sql_text := concat( 'SELECT Object.* FROM Object WHERE Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), '''); ');
    PREPARE stmt FROM @sql_text;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;

   IF( IncludeMetadata = 1 ) THEN
        SET @sql_text := concat( 'SELECT M1.* ',
                                 'FROM  Object ' ,
                                       'JOIN Metadata AS M1 ON Object.GUID = M1.ObjectGUID AND ',
                                        'M1.GUID = ( SELECT GUID ',
                                                      'FROM Metadata AS M2 ',
                                                     'WHERE M2.ObjectGUID         = M1.ObjectGUID   AND ',
                                                           'M2.LanguageCode       = M1.LanguageCode AND ',
                                                           'M2.MetadataSchemaGUID = M1.MetadataSchemaGUID ',
                                                     'ORDER BY M2.RevisionID DESC ',
                                                     'LIMIT 1) ',
                                 'WHERE Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), '''); ');
        PREPARE stmt FROM @sql_text;
        EXECUTE stmt;
        DEALLOCATE PREPARE stmt;
    END IF;

    IF( IncludeFiles = 1 ) THEN
        SET @sql_text := concat( 'SELECT FileInfo.* FROM Object INNER JOIN FileInfo ON Object.GUID = FileInfo.ObjectGUID WHERE Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), '''); ');
        PREPARE stmt FROM @sql_text;
        EXECUTE stmt;
        DEALLOCATE PREPARE stmt;    
    END IF;
                
    IF( IncludeObjectRelations = 1 ) THEN
        SET @sql_text := concat( 'SELECT DISTINCT Object_Object_Join.* FROM Object INNER JOIN  Object_Object_Join ON Object_Object_Join.Object1GUID = Object.GUID OR Object_Object_Join.Object2GUID = Object.GUID WHERE Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), '''); ');
        PREPARE stmt FROM @sql_text;
        EXECUTE stmt;
        DEALLOCATE PREPARE stmt;
    END IF;
    
    IF( IncludeFolders = 1 ) THEN
        SET @sql_text := concat( 'SELECT Object_Folder_Join.* FROM Object INNER JOIN Object_Folder_Join ON Object.GUID = Object_Folder_Join.ObjectGUID WHERE Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), '''); ');
        PREPARE stmt FROM @sql_text;
        EXECUTE stmt;
        DEALLOCATE PREPARE stmt;
    END IF;
    
    IF( IncludeAccessPoints = 1 ) THEN
        SET @sql_text := concat( 'SELECT AccessPoint_Object_Join.* FROM Object INNER JOIN AccessPoint_Object_Join ON Object.GUID = AccessPoint_Object_Join.ObjectGUID WHERE Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), '''); ');
        PREPARE stmt FROM @sql_text;
        EXECUTE stmt;
        DEALLOCATE PREPARE stmt;
    END IF;

END