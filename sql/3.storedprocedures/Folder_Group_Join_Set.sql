CREATE PROCEDURE Folder_Group_Join_Set
(
    GroupGuid   BINARY(16),
    FolderID    INT UNSIGNED,
    Permission  INT UNSIGNED
)
BEGIN

    IF( SELECT 
            COUNT(*)
        FROM 
            Folder_Group_Join AS FGJ
        WHERE 
                FGJ.GroupGUID = GroupGuid
            AND FGJ.FolderID  = FolderID ) = 0 
    THEN

        INSERT INTO Folder_Group_Join
            ( FolderID, GroupGUID, Permission, DateCreated) 
        VALUES
            ( FolderID, GroupGuid, Permission, NOW() );

    ELSE

        UPDATE 
            Folder_Group_Join AS FGJ
        SET 
            FGJ.Permission = Permission
        WHERE 
                FGJ.GroupGUID = GroupGuid
            AND FGJ.FolderID  = FolderID;

    END IF;
    
    SELECT ROW_COUNT();
END
