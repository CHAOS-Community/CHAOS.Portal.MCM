CREATE PROCEDURE Folder_Update
(
    IN  ID              INT,
    IN  NewName         VARCHAR(255),
    IN  NewParentID     INT,
    IN  NewFolderTypeID INT
)
BEGIN

    UPDATE	
        Folder
    SET	
        Folder.ParentID     = IFNULL( NewParentID, Folder.ParentID),
        Folder.FolderTypeID = IFNULL( NewFolderTypeID, Folder.FolderTypeID ),
        Folder.Name         = IFNULL( NewName, Folder.Name )
    WHERE
        Folder.ID = ID;

    SELECT ROW_COUNT();

END