CREATE PROCEDURE Folder_User_Join_Set
(
    UserGUID    BINARY(16),
    FolderID    INT UNSIGNED,
    Permission  INT UNSIGNED
)
BEGIN

    IF( SELECT 
            COUNT(*)
        FROM 
            Folder_User_Join AS FUJ
        WHERE
            FUJ.UserGUID = UserGUID AND
            FUJ.FolderID = FolderID ) = 0 
    THEN

        INSERT INTO Folder_User_Join
            ( FolderID, UserGUID, Permission, DateCreated) 
        VALUES
            ( FolderID, UserGUID, Permission, NOW() );

    ELSE

        UPDATE 
            Folder_User_Join AS FUJ
        SET 
            FUJ.Permission = Permission
        WHERE 
                FUJ.UserGUID = UserGUID
            AND FUJ.FolderID = FolderID;

    END IF;

    SELECT ROW_COUNT();

END