CREATE PROCEDURE `MetadataSchema_Get`(
    UserGuid            BINARY(16),
    GroupGuids          VARCHAR(21845),
    MetadataSchemaGuid  BINARY(16),
    PermissionRequired  INTEGER UNSIGNED

)
BEGIN
    SET @sql_text := concat( 'SELECT  MS.* ',

                               'FROM  MetadataSchema AS MS ' ,

                                     'LEFT OUTER JOIN MetadataSchema_Group_Join ON MS.GUID = MetadataSchema_Group_Join.MetadataSchemaGUID ',

                                     'LEFT OUTER JOIN MetadataSchema_User_Join  ON MS.GUID = MetadataSchema_User_Join.MetadataSchemaGUID ',

                              'WHERE  (1=1) ');
                              
                              

    IF( MetadataSchemaGuid IS NOT NULL ) THEN

        SET @sql_text := concat( @sql_text, 

                                     'AND (unhex(''',hex(MetadataSchemaGuid),''') = MS.GUID ) ' );

    END IF;
    
    IF( GroupGuids IS NOT NULL ) THEN

        SET @sql_text := concat( @sql_text, 

                                     'AND '

                                     '((  ',

                                        'MetadataSchema_Group_Join.Permission & ',PermissionRequired,' = ',PermissionRequired,' AND '

                                        '(MetadataSchema_Group_Join.GroupGUID = unhex(''', REPLACE(GroupGuids,',',''') OR MetadataSchema_Group_Join.GroupGUID = unhex('''), ''')) ',

                                     ') OR ',

                                     '(',

                                         'MetadataSchema_User_Join.UserGUID = unhex(''',hex(UserGuid),''') AND ',

                                         'MetadataSchema_User_Join.Permission & ',PermissionRequired,' = ',PermissionRequired,



                                     ')); ');



    END IF;
    
    IF( UserGuid IS NOT NULL ) THEN

        SET @sql_text :=  concat( @sql_text,

                                     'AND ',

                                     '(',

                                         'MetadataSchema_User_Join.UserGUID = unhex(''',hex(UserGuid),''') AND ',

                                         'MetadataSchema_User_Join.Permission & ',PermissionRequired,' = ',PermissionRequired,

                                     '); ');

    END IF;

    PREPARE stmt FROM @sql_text;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
END