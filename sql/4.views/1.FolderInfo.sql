CREATE VIEW FolderInfo 
AS 
	SELECT 
		f.ID AS ID,
		f.ParentID AS ParentID,
		f.FolderTypeID AS FolderTypeID,
		f.SubscriptionGUID AS SubscriptionGUID,
		f.Name AS Name,
		f.DateCreated AS DateCreated,
		(SELECT count(*) FROM Folder WHERE Folder.ParentID = f.ID) AS NumberOfSubFolders,
		(SELECT count(*) FROM Object_Folder_Join WHERE Object_Folder_Join.FolderID = f.ID) AS NumberOfObjects 
	FROM
		Folder f;

