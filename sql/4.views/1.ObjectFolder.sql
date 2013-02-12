CREATE VIEW ObjectFolder
AS 
	SELECT 
		f.ID AS ID,
		f.ParentID AS ParentID,
		f.FolderTypeID AS FolderTypeID,
		ofj.ObjectGUID AS ObjectGuid,
		ofj.ObjectFolderTypeID,
		f.SubscriptionGUID AS SubscriptionGUID,
		f.Name AS Name,
		f.DateCreated AS DateCreated
	FROM
		Folder AS f
		INNER JOIN Object_Folder_Join AS ofj ON f.ID = ofj.FolderID;

