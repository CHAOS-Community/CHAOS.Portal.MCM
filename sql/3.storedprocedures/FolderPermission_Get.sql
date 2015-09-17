CREATE PROCEDURE FolderPermission_Get
(

)
BEGIN

	SELECT
		*
	FROM
		Folder_User_Join;

	SELECT
		*
	FROM
		Folder_Group_Join;

END