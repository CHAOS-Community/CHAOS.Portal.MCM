CREATE PROCEDURE Folder_Get_DirectFolderAssociations(
    GroupGUIDs          VARCHAR(21845),
    UserGUID			      BINARY(16),
    RequiredPermission	INT
)
BEGIN

    SELECT 
        Folder.*
    FROM 
        Folder 
        LEFT OUTER JOIN Folder_User_Join  ON Folder.ID = Folder_User_Join.FolderID
        LEFT OUTER JOIN Folder_Group_Join ON Folder.ID = Folder_Group_Join.FolderID
     WHERE	
        Folder_User_Join.UserGUID = UserGUID OR 
        GroupGUIDs LIKE CONCAT( '%', HEX( Folder_Group_Join.GroupGUID ), '%' )
        AND
        ( 
            RequiredPermission IS NULL
            OR
            ( 
                Folder_User_Join.Permission  & RequiredPermission = RequiredPermission OR
                Folder_Group_Join.Permission & RequiredPermission = RequiredPermission 
            ) 
        );

END