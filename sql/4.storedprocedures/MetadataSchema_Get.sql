CREATE PROCEDURE MetadataSchema_Get
(
    IN  UserGUID            BINARY(16),
    IN  GroupGUIDs          VARCHAR(21845),
    IN  MetadataSchemaGUID  BINARY(16),
    IN  PermissionRequired  INTEGER UNSIGNED
)
BEGIN
    
    SET @sql_text := concat( 'SELECT  MS.* ',
                               'FROM  MetadataSchema AS MS ' ,
                                     'LEFT OUTER JOIN MetadataSchema_Group_Join ON MS.GUID = MetadataSchema_Group_Join.MetadataSchemaGUID ',
                                     'LEFT OUTER JOIN MetadataSchema_User_Join  ON MS.GUID = MetadataSchema_User_Join.MetadataSchemaGUID ',
                              'WHERE  (1=1) ');
    IF( MetadataSchemaGUID IS NOT NULL ) THEN
        SET @sql_text := concat( @sql_text, 
                                     'AND (unhex(''',hex(MetadataSchemaGUID),''') = MS.GUID ) ' );
    END IF;
    IF( GroupGUIDs IS NOT NULL ) THEN
        SET @sql_text := concat( @sql_text, 
                                     'AND '
                                     '((  ',
                                        'MetadataSchema_Group_Join.Permission & ',PermissionRequired,' = ',PermissionRequired,' AND '
                                        '(MetadataSchema_Group_Join.GroupGUID = unhex(''', REPLACE(GroupGUIDs,',',''') OR MetadataSchema_Group_Join.GroupGUID = unhex('''), ''')) ',
                                     ') OR ',
                                     '(',
                                         'MetadataSchema_User_Join.UserGUID = unhex(''',hex(UserGUID),''') AND ',
                                         'MetadataSchema_User_Join.Permission & ',PermissionRequired,' = ',PermissionRequired,

                                     ')); ');

    ELSEIF( UserGUID IS NOT NULL ) THEN
        SET @sql_text :=  concat( @sql_text,
                                     'AND ',
                                     '(',
                                         'MetadataSchema_User_Join.UserGUID = unhex(''',hex(UserGUID),''') AND ',
                                         'MetadataSchema_User_Join.Permission & ',PermissionRequired,' = ',PermissionRequired,
                                     '); ');
    ELSE
        SET @sql_text :=  concat( @sql_text,
                                     'AND ',
                                     '(',
                                         'MetadataSchema_User_Join.Permission & ',PermissionRequired,' = ',PermissionRequired,
                                     '); ');
    END IF;
    
    PREPARE stmt FROM @sql_text;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;

END