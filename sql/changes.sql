-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.10.13
--				This SP is used to get metadatas
-- =============================================
CREATE PROCEDURE Metadata_Get
	@ObjectGUID			uniqueidentifier,
	@MetadataSchemaGUID uniqueidentifier = NULL,
	@LanguageID			int              = NULL
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @ObjectID INT
	SELECT	@ObjectID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @ObjectGUID

	DECLARE @MetadataSchemaID INT
	SELECT	@MetadataSchemaID = ID
	  FROM	MetadataSchema
	 WHERE	MetadataSchema.[GUID] = @MetadataSchemaGUID

	SELECT	*
	  FROM	Metadata
	 WHERE	Metadata.ObjectID = @ObjectID AND
			( @MetadataSchemaGUID IS NULL OR @MetadataSchemaID = Metadata.MetadataSchemaID ) AND
			( @LanguageID IS NULL OR @LanguageID = Metadata.LanguageID )
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.19
--				This SP is used to delete objects
-- =============================================
ALTER PROCEDURE [dbo].[Object_Delete]
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
			
	DELETE 
	  FROM	[Metadata]
     WHERE	ObjectID = @ObjectID
			
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
