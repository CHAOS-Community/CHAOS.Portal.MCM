CREATE PROCEDURE Folder_User_Join_Set
(
    UserGuid    BINARY(16),
    FolderID    INT UNSIGNED,
    Permission  INT UNSIGNED
)
BEGIN

    IF NOT EXISTS( SELECT 
						*
					FROM 
						Folder_User_Join AS FUJ
					WHERE
						FUJ.UserGUID = UserGuid AND
						FUJ.FolderID = FolderID )
    THEN

        INSERT INTO Folder_User_Join
            ( FolderID, UserGUID, Permission, DateCreated) 
        VALUES
            ( FolderID, UserGuid, Permission, NOW() );

    ELSE

        UPDATE 
            Folder_User_Join AS FUJ
        SET 
            FUJ.Permission = Permission
        WHERE 
                FUJ.UserGUID = UserGuid
            AND FUJ.FolderID = FolderID;

    END IF;

    SELECT ROW_COUNT();

END