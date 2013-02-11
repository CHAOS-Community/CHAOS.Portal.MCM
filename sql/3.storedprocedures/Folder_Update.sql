CREATE PROCEDURE Folder_Update
(
	ID              INT,
	NewName         VARCHAR(255),
	NewParentID     INT,
	NewFolderTypeID INT
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