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

    SET @sql_text := concat( 'SELECT OM.* ',
                                'FROM  ObjectMetadata AS OM ' ,
                            --        'JOIN Metadata AS M1 ON omj1.MetadataGuid = M1.Guid ',
                            --          ' AND M1.GUID = ( SELECT GUID ',
                            --                        'FROM Metadata AS M2 ',
                            --                       'WHERE M2.ObjectGUID         = M1.ObjectGUID   AND ',
                            --                             'M2.LanguageCode       = M1.LanguageCode AND ',
                            --                             'M2.MetadataSchemaGUID = M1.MetadataSchemaGUID ',
                            --                       'ORDER BY M2.RevisionID DESC ',
                            --                       'LIMIT 1) ',
                                'WHERE ', IncludeMetadata ,' = 1 AND ( OM.ObjectGuid = unhex(''', REPLACE(GUIDs,',',''') OR OM.ObjectGuid = unhex('''), ''') ); ');
    PREPARE stmt FROM @sql_text;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;

    SET @sql_text := concat( 'SELECT FileInfo.* FROM Object INNER JOIN FileInfo ON Object.GUID = FileInfo.ObjectGUID WHERE ', IncludeFiles ,' = 1 AND ( Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), ''') ); ');
    PREPARE stmt FROM @sql_text;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;    
                

    SET @sql_text := concat( 'SELECT DISTINCT Object_Object_Join.* FROM Object INNER JOIN  Object_Object_Join ON Object_Object_Join.Object1GUID = Object.GUID OR Object_Object_Join.Object2GUID = Object.GUID WHERE ', IncludeObjectRelations ,' = 1 AND ( Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), ''') ); ');
    PREPARE stmt FROM @sql_text;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    
    SET @sql_text := concat( 'SELECT Object_Folder_Join.* FROM Object INNER JOIN Object_Folder_Join ON Object.GUID = Object_Folder_Join.ObjectGUID WHERE ', IncludeFolders ,' = 1 AND ( Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), ''') ); ');
    PREPARE stmt FROM @sql_text;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    
    SET @sql_text := concat( 'SELECT AccessPoint_Object_Join.* FROM Object INNER JOIN AccessPoint_Object_Join ON Object.GUID = AccessPoint_Object_Join.ObjectGUID WHERE ', IncludeAccessPoints ,' = 1 AND ( Object.GUID = unhex(''', REPLACE(GUIDs,',',''') OR Object.GUID = unhex('''), ''') ); ');
    PREPARE stmt FROM @sql_text;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;

END