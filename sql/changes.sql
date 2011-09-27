-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Get Objects
-- =============================================
CREATE PROCEDURE Object_Get
	@GUIDs		GUIDList Readonly,
	@GroupGUIDs	GUIDList Readonly,
	@UserGUID	uniqueidentifier,
	@ObjectID	int					= null,
	@FolderID	int					= null
	
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'GET_OBJECTS' )

	SELECT	o.*
	  FROM	[Object] as o INNER JOIN
				Object_Folder_Join ON o.ID = Object_Folder_Join.ObjectID
	 WHERE	( @FolderID IS NULL OR Object_Folder_Join.FolderID = @FolderID ) AND
			( o.[GUID] in ( SELECT g.[GUID] FROM @GUIDs as g ) OR ( @ObjectID IS NULL OR o.ID = @ObjectID ) ) AND
			dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,Object_Folder_Join.FolderID ) & @RequiredPermission = @RequiredPermission

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Create Objects
-- =============================================
CREATE PROCEDURE Object_Create
	@GroupGUIDs		GUIDList Readonly,
	@UserGUID		uniqueidentifier,
	@GUID			uniqueidentifier	= null,
	@ObjectTypeID	int,
	@FolderID		int
AS
BEGIN
	
	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'CREATE_UPDATE_OBJECTS' )

	IF( @RequiredPermission & dbo.Folder_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@FolderID ) <> @RequiredPermission )
			RETURN -100

	IF( @GUID IS NULL )
		SET @GUID = NEWID()

	BEGIN TRANSACTION 

	INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated])
		 VALUES (@GUID,@ObjectTypeID,GETDATE())
		 
	DECLARE @ObjectID INT
    SET @ObjectID = @@IDENTITY 
		 
	INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated])
		 VALUES (@ObjectID,@FolderID,0,GETDATE())
		 
	IF( @@ERROR <> 0 )
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
		 
	RETURN @ObjectID

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.19
--				This SP is used to delete objects
-- =============================================
CREATE PROCEDURE Object_Delete
	@GroupGUIDs		GUIDList Readonly,
	@UserGUID		uniqueidentifier,
	@GUID			uniqueidentifier,
	@FolderID		int
AS
BEGIN
	
	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'DELETE_OBJECTS' )
	
	IF( @RequiredPermission & dbo.Folder_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@FolderID ) <> @RequiredPermission )
			RETURN -100
	
	BEGIN TRANSACTION
	
	DECLARE	@ObjectID INT
	SELECT @ObjectID = ID FROM [Object] WHERE [GUID] = @GUID
	
	DELETE
	  FROM	Object_Folder_Join
	 WHERE	FolderID = @FolderID AND
			ObjectID = @ObjectID
			
	-- Delete object, this should be changed when links are implemented	
	DELETE
	  FROM	[Object]
	 WHERE	ID = @ObjectID
	
	IF( @@ERROR <> 0 )
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
	
	RETURN 1
	
END
GO


ALTER PROCEDURE [dbo].[Folder_Update]
	@GroupGUIDs			GUIDList READONLY,
	@UserGUID			uniqueidentifier,
	@ID					int,
	@NewTitle			varchar(255) = null,
	@NewParentID		int          = null,
	@NewFolderTypeID	int			 = null
AS
BEGIN

	if( @NewTitle IS NULL AND @NewFolderTypeID IS NULL AND @NewParentID IS NULL )
		RETURN -10

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = 0
	
	-- OR with general UPDATE permission if applies
	if( @NewTitle IS NOT NULL OR @NewFolderTypeID IS NOT NULL )
		SET @RequiredPermission = @RequiredPermission | dbo.GetPermissionForAction( 'Folder', 'UPDATE' )
	
	-- OR with MOVE permission if applies
	if( @NewParentID IS NOT NULL )
		SET @RequiredPermission = @RequiredPermission | dbo.GetPermissionForAction( 'Folder', 'MOVE_FOLDER' )
	
	IF( dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,@ID ) & @RequiredPermission <> @RequiredPermission ) 
		RETURN -100

	UPDATE	Folder
	   SET	[ParentID] = @NewParentID
			,[FolderTypeID] = ISNULL(@NewFolderTypeID,[FolderTypeID])
			,[Title] = ISNULL(@NewTitle,[Title])
	 WHERE	Folder.ID = @ID

	RETURN @@ROWCOUNT
	
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.20
--				This SP is used to move objects into new folders
-- =============================================
CREATE PROCEDURE Object_Folder_Join_Update
	@GroupGUIDs		GUIDList READONLY,
	@UserGUID		uniqueidentifier,
	@ObjectID		int,
	@FolderID		int,
	@NewFolderID	int
AS
BEGIN
	
	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'CREATE_UPDATE_OBJECTS' )
	
	IF( dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,@FolderID ) & @RequiredPermission <> @RequiredPermission ) 
		RETURN -100
	
	UPDATE	Object_Folder_Join
	   SET	FolderID = @NewFolderID
	 WHERE	ObjectID = @ObjectID AND
			FolderID = @FolderID
			
	RETURN @@ROWCOUNT
	
END
GO
