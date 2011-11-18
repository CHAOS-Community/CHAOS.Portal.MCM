-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.10.04
--				This function given a objectID will return the User and Groups accumulated permission
-- =============================================
ALTER FUNCTION [dbo].[Object_FindHighestUserPermission]
(
	@UserGUID	uniqueidentifier,
	@GroupGUIDs	GUIDList READONLY,
	@ObjectID	INT
)
RETURNS INT
AS
BEGIN
	
	
	
	DECLARE @FolderPermissions TABLE
	(
	  RowNum int,
	  Permission int
	)
	
	-- Find all folders the users / groups has direct access to
	INSERT INTO	@FolderPermissions
		SELECT	ROW_NUMBER() OVER( ORDER BY ID ), dbo.Folder_FindHighestUserPermission( @UserGUID,@GroupGUIDs,FolderID )
		  FROM	[Object] INNER JOIN
					Object_Folder_Join ON [Object].ID = Object_Folder_Join.ObjectID
		 WHERE	[Object].ID = @ObjectID
	
	DECLARE	@Permission	INT
	DECLARE @MaxCount INT
	SET @Permission = 0
	
	SELECT	@MaxCount = COUNT(*)
	  FROM	@FolderPermissions
	
	-- Traverse through the permissions, and "OR" all permissions, to find the highest
	WHILE( @MaxCount > 0 )
	BEGIN

		SELECT	@Permission = Permission | @Permission
		  FROM	@FolderPermissions
		  WHERE	RowNum = @MaxCount
		  
		SET @MaxCount = @MaxCount - 1
		  
	END

	RETURN @Permission

END
GO

CREATE NONCLUSTERED INDEX [_dta_index_Object_5_482100758__K2_K1_3_6] ON [dbo].[Object] 
(
	[GUID] ASC,
	[ID] ASC
)
INCLUDE ( [ObjectTypeID],
[DateCreated]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [_dta_index_Metadata_5_1630628852__K2_1_3_4_5_6_7_8_9] ON [dbo].[Metadata] 
(
	[ObjectID] ASC
)
INCLUDE ( [ID],
[LanguageCode],
[MetadataSchemaID],
[MetadataXml],
[DateCreated],
[DateModified],
[DateLocked],
[LockUserGUID]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
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
	@FolderID			int					= null
	--@PageIndex			int					= 0,
	--@PageSize			int					= 10,
	--@TotalCount			int	output
	
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'GET_OBJECTS' )

	--IF( @PageIndex IS NULL )
	--	SET @PageIndex = 0
		
	--IF( @PageSize IS NULL )
	--	SET @PageSize = 10;

	--DECLARE @PagedResults AS TABLE (
	--	[RowNumber]		int,
	--	[TotalCount]	int,
	--    [ObjectID]		int
	--);
	
	DECLARE @Results AS TABLE (
	    [ObjectID]		int
	);

	--WITH ObjectsRN AS
	--(
	IF NOT EXISTS( SELECT * FROM @GUIDs )
		INSERT INTO @Results
			SELECT	[Object].ID
			  FROM	[Object]
			 WHERE	( @ObjectID IS NULL OR [Object].ID = @ObjectID )
	ELSE
		INSERT INTO @Results
			SELECT	[Object].ID
			  FROM	@GUIDs as g INNER JOIN
						[Object] ON g.GUID = [Object].GUID
			 WHERE	( @ObjectID IS NULL OR [Object].ID = @ObjectID )
		--SELECT	o.ID
		-- FROM	[Object] as o 
		-- WHERE	--( @FolderID IS NULL OR Object_Folder_Join.FolderID = @FolderID ) AND
		--		--( @ObjectTypeID IS NULL OR o.ObjectTypeID = @ObjectTypeID ) AND
		--		( ( SELECT COUNT(*) FROM @GUIDs as g ) = 0 OR o.[GUID] in ( SELECT g.[GUID] FROM @GUIDs as g ) ) AND
		--		( @ObjectID IS NULL OR o.ID = @ObjectID )-- AND
			--	dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,Object_Folder_Join.FolderID ) & @RequiredPermission = @RequiredPermission
	--)

	--INSERT INTO	@PagedResults
	--	 SELECT	* 
	--	   FROM	ObjectsRN
	--	  WHERE RowNumber BETWEEN (@PageIndex)     * @PageSize + 1 
	--				          AND (@PageIndex + 1) * @PageSize

	SELECT	*
	  FROM	[Object]
	 WHERE	ID in (SELECT g.ObjectID FROM @Results as g)--( SELECT pr.ObjectID FROM @PagedResults as pr )
	 
	 if( @IncludeMetadata = 1 )
		SELECT	*
		  FROM	Metadata
		 WHERE	Metadata.ObjectID IN (SELECT g.ObjectID FROM @Results as g)-- ( SELECT pr.ObjectID FROM @PagedResults as pr )
		 
	 if( @IncludeFiles = 1 )
		 SELECT	*
		  FROM	[File]
		 WHERE	[File].ObjectID IN (SELECT g.ObjectID FROM @Results as g)-- ( SELECT pr.ObjectID FROM @PagedResults as pr )

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.10.04
--				This SP creates or updates metadata
-- =============================================
ALTER PROCEDURE [dbo].[Metadata_Set]
	@GroupGUIDs		    GUIDList Readonly,
	@UserGUID		    uniqueidentifier,
	@ObjectGUID			uniqueidentifier,
	@MetadataSchemaGUID uniqueidentifier,
	@LanguageCode		varchar(10),
	@MetadataXML		xml,
	@Lock				bit = NULL
AS
BEGIN
	
	DECLARE @ObjectID         INT
	DECLARE @MetadataSchemaID INT
	
	SELECT	@ObjectID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @ObjectGUID
	 
	SELECT	@MetadataSchemaID = ID
	  FROM	MetadataSchema
	 WHERE	[GUID] = @MetadataSchemaGUID
	
	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Metadata', 'CREATE_UPDATE_OBJECTS' )

	IF( @RequiredPermission & dbo.Object_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@ObjectID ) <> @RequiredPermission )
			RETURN -100
	
	IF EXISTS( SELECT ID FROM Metadata WHERE ObjectID = @ObjectID AND MetadataSchemaID = @MetadataSchemaID AND LanguageCode = @LanguageCode )
	BEGIN
	
		DECLARE @DateLocked	DATETIME
		DECLARE @LockUserGUID UNIQUEIDENTIFIER
	
		SELECT	@DateLocked = GETDATE(),
				@LockUserGUID = @UserGUID
		 WHERE	@Lock = 1
	
		UPDATE [Metadata]
		   SET [MetadataXml]  = @MetadataXML,
		       [DateModified] = GETDATE(),
		       [DateLocked]   = ISNULL(@DateLocked,[DateLocked]),
		       [LockUserGUID] = ISNULL(@LockUserGUID,[LockUserGUID])
		WHERE  ObjectID         = @ObjectID AND 
			   MetadataSchemaID = @MetadataSchemaID AND 
			   LanguageCode     = @LanguageCode
	
		RETURN @@ROWCOUNT
	
	END
	ELSE 
	BEGIN
			
		INSERT INTO [Metadata]([ObjectID],LanguageCode,[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])
			 VALUES (@ObjectID,@LanguageCode,@MetadataSchemaID,@MetadataXML,GETDATE(),GETDATE(),null,null )

		RETURN @@ROWCOUNT
		
	END
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Get Objects
-- =============================================
CREATE PROCEDURE [dbo].[Object_GetByGUIDs]
	@GUIDs				GUIDList Readonly,
	@IncludeMetadata	bit,
	@IncludeFiles		bit
	
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @Results AS TABLE (
	    [ObjectID]		int
	);
	INSERT INTO @Results
		SELECT	[Object].ID
		  FROM	@GUIDs as g INNER JOIN
					[Object] ON g.GUID = [Object].GUID
		
	SELECT	*
	  FROM	[Object]
	 WHERE	ID in (SELECT g.ObjectID FROM @Results as g)--( SELECT pr.ObjectID FROM @PagedResults as pr )
	 
	 if( @IncludeMetadata = 1 )
		SELECT	*
		  FROM	Metadata
		 WHERE	Metadata.ObjectID IN (SELECT g.ObjectID FROM @Results as g)-- ( SELECT pr.ObjectID FROM @PagedResults as pr )
		 
	 if( @IncludeFiles = 1 )
		 SELECT	*
		  FROM	[File]
		 WHERE	[File].ObjectID IN (SELECT g.ObjectID FROM @Results as g)-- ( SELECT pr.ObjectID FROM @PagedResults as pr )

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
	@PageSize			int					= 10
	--@TotalCount			int	output
	
AS
BEGIN

	SET NOCOUNT ON;

	IF( @PageIndex IS NULL )
		SET @PageIndex = 0
		
	IF( @PageSize IS NULL )
		SET @PageSize = 10;

	DECLARE @PagedResults AS TABLE (
		[RowNumber]		int,
	    [ObjectID]		int
	);
	
	IF EXISTS( SELECT * FROM @GUIDs )	
	BEGIN
		WITH ObjectsRN AS
		(
			SELECT	ROW_NUMBER() OVER(ORDER BY [Object].[GUID]) AS RowNumber, 
					[Object].ID
			  FROM	@GUIDs as g LEFT OUTER JOIN
						[Object] ON g.GUID = [Object].GUID
			 WHERE	( @ObjectID IS NULL OR [Object].ID = @ObjectID )
		)
			
		INSERT INTO	@PagedResults
			 SELECT	* 
			   FROM	ObjectsRN
			  WHERE RowNumber BETWEEN (@PageIndex)     * @PageSize + 1 
								  AND (@PageIndex + 1) * @PageSize
	END
	ELSE
	BEGIN
		DECLARE	@RequiredPermission	int
		SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'GET_OBJECTS' );
	 
		WITH ObjectsRN AS
		(
			SELECT	ROW_NUMBER() OVER(ORDER BY [Object].[GUID]) AS RowNumber, 
					[Object].ID
			  FROM	[Object] INNER JOIN Object_Folder_Join
					ON [Object].ID = Object_Folder_Join.ObjectID
			 WHERE	( @ObjectID IS NULL OR [Object].ID = @ObjectID ) AND
					( @FolderID IS NULL OR Object_Folder_Join.FolderID = @FolderID ) AND
					( @ObjectTypeID IS NULL OR [Object].ObjectTypeID = @ObjectTypeID ) AND
					dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,Object_Folder_Join.FolderID ) & @RequiredPermission = @RequiredPermission
		)
			
		INSERT INTO	@PagedResults
			 SELECT	* 
			   FROM	ObjectsRN
			  WHERE RowNumber BETWEEN (@PageIndex)     * @PageSize + 1 
								  AND (@PageIndex + 1) * @PageSize
	END

	SELECT	*
	  FROM	[Object]
	 WHERE	ID in (SELECT g.ObjectID FROM @PagedResults as g)
	 
	 if( @IncludeMetadata = 1 )
		SELECT	*
		  FROM	Metadata
		 WHERE	Metadata.ObjectID IN (SELECT g.ObjectID FROM @PagedResults as g)
		 
	 if( @IncludeFiles = 1 )
		 SELECT	*
		  FROM	[File]
		 WHERE	[File].ObjectID IN (SELECT g.ObjectID FROM @PagedResults as g)

END
