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
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Get Objects
-- =============================================
ALTER PROCEDURE [dbo].[Object_Get]
	@GUIDs				GUIDList Readonly,
	@GroupGUIDs			GUIDList Readonly,
	@UserGUID			uniqueidentifier,
	@IncludeMetadata	bit,
	@IncludeFiles		bit,
	@ObjectID			int					= null,
	@ObjectTypeID		int					= null,
	@FolderID			int					= null,
	@PageIndex			int					= 0,
	@PageSize			int					= 10,
	@TotalCount			int	output
	
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'GET_OBJECTS' )

	IF( @PageIndex IS NULL )
		SET @PageIndex = 0
		
	IF( @PageSize IS NULL )
		SET @PageSize = 10;

	DECLARE @PagedResults AS TABLE (
		[RowNumber]		int,
		[TotalCount]	int,
	    [ObjectID]		int
	);

	WITH ObjectsRN AS
	(
		SELECT	ROW_NUMBER() OVER(ORDER BY o.[GUID], o.[GUID]) AS RowNumber,
				COUNT(*) OVER() AS TotalCount,
				o.ID
		 FROM	[Object] as o INNER JOIN
				Object_Folder_Join ON o.ID = Object_Folder_Join.ObjectID
		 WHERE	( @FolderID IS NULL OR Object_Folder_Join.FolderID = @FolderID ) AND
				( @ObjectTypeID IS NULL OR o.ObjectTypeID = @ObjectTypeID ) AND
				( o.[GUID] in ( SELECT g.[GUID] FROM @GUIDs as g ) OR ( @ObjectID IS NULL OR o.ID = @ObjectID ) ) AND
				dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,Object_Folder_Join.FolderID ) & @RequiredPermission = @RequiredPermission
	)

	INSERT INTO	@PagedResults
		 SELECT	* 
		   FROM	ObjectsRN
		  WHERE RowNumber BETWEEN (@PageIndex)     * @PageSize + 1 
					          AND (@PageIndex + 1) * @PageSize

	SELECT	*
	  FROM	[Object]
	 WHERE	ID in ( SELECT pr.ObjectID FROM @PagedResults as pr )
	 
	 if( @IncludeMetadata = 1 )
		SELECT	*
		  FROM	Metadata
		 WHERE	Metadata.ObjectID IN ( SELECT pr.ObjectID FROM @PagedResults as pr )
		 
	 if( @IncludeFiles = 1 )
		 SELECT	*
		  FROM	[File]
		 WHERE	[File].ObjectID IN ( SELECT pr.ObjectID FROM @PagedResults as pr )

END
