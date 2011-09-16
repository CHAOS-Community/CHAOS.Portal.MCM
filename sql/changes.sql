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
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'GET_OBJECT' )

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
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'CREATE_OBJECT' )

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
