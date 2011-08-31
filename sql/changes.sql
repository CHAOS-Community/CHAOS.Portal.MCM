-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.31
--				This SP is used to delete a folder
-- =============================================
CREATE PROCEDURE Folder_Delete
	@GroupGUIDs		GUIDList READONLY,
	@UserGUID		uniqueidentifier,
	@ID				int
AS
BEGIN

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'DELETE' )

	IF( dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,@ID ) & @RequiredPermission <> @RequiredPermission ) 
		RETURN -100

	IF EXISTS( SELECT * FROM Folder WHERE Folder.ParentID = @ID )
		RETURN -50
		
	IF EXISTS( SELECT * FROM Object_Folder_Join WHERE FolderID = @ID )
		RETURN -50
		
	BEGIN TRANSACTION Delete_Folder
	
	DELETE 
	  FROM	[Folder_Group_Join]
     WHERE	FolderID = @ID

	DELETE 
	  FROM	[Folder_User_Join]
     WHERE	FolderID = @ID
     
     DELETE 
       FROM	[Folder]
      WHERE	ID = @ID

	IF( @@ERROR = 0 )
		COMMIT TRANSACTION Delete_Folder
	ELSE
		ROLLBACK TRANSACTION Delete_Folder
		
	RETURN 1
	
END
GO
