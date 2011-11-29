-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.11.24
--				This SP is used to get Metadata Schemas
-- =============================================
CREATE PROCEDURE MetadataSchema_Get
	@ID		int	= NULL
AS
BEGIN

	SET NOCOUNT ON;

    SELECT	[ID],[GUID],[name],[SchemaXml],[DateCreated]
	  FROM	[MetadataSchema]
	 WHERE	( @ID IS NULL OR @ID = ID )
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
	@MetadataSchemaID	int,
	@LanguageCode		varchar(10),
	@MetadataXML		xml,
	@Lock				bit = NULL
AS
BEGIN
	
	DECLARE @ObjectID         INT
	
	SELECT	@ObjectID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @ObjectGUID
	
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
-- Create date: 2011.11.24
--				This SP is used to create a relation between two objects
-- =============================================
CREATE PROCEDURE ObjectRelation_Create
	@GroupGUIDs				GUIDList Readonly,
	@UserGUID				uniqueidentifier,
	@Object1GUID			uniqueidentifier,
	@Object2GUID			uniqueidentifier,
	@ObjectRelationTypeID	int,
	@Sequence				int	= null
AS
BEGIN

	DECLARE @Object1ID INT
	DECLARE @Object2ID INT
	
	SELECT	@Object1ID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @Object1GUID

	SELECT	@Object2ID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @Object2GUID

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Metadata', 'CREATE_UPDATE_OBJECTS' )

	IF( @RequiredPermission & dbo.Object_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@Object1ID ) <> @RequiredPermission )
			RETURN -100

	IF EXISTS( SELECT ObjectID1 
	             FROM [Object_Object_Join]
	            WHERE ObjectID1 = @Object1ID AND 
					  ObjectID2 = @Object2ID AND
			          ObjectRelationTypeID = @ObjectRelationTypeID  )
		RETURN -200

	INSERT INTO [Object_Object_Join] ([ObjectID1],[ObjectID2],[ObjectRelationTypeID],[Sequence],[DateCreated])
		 VALUES (@Object1ID,@Object2ID,@ObjectRelationTypeID,@Sequence,GETDATE())
		 
	RETURN @@ROWCOUNT

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.11.28
--				This SP is used to Delete an ObjectRelation
-- =============================================
CREATE PROCEDURE ObjectRelation_Delete
	@GroupGUIDs				GUIDList Readonly,
	@UserGUID				uniqueidentifier,
	@Object1GUID			uniqueidentifier,
	@Object2GUID			uniqueidentifier,
	@ObjectRelationTypeID	int
AS
BEGIN

	DECLARE @Object1ID INT
	DECLARE @Object2ID INT
	
	SELECT	@Object1ID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @Object1GUID

	SELECT	@Object2ID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @Object2GUID

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Metadata', 'CREATE_UPDATE_OBJECTS' )

	IF( @RequiredPermission & dbo.Object_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@Object1ID ) <> @RequiredPermission )
			RETURN -100

	DELETE
	  FROM	Object_Object_Join
	 WHERE	ObjectID1 = @Object1ID AND 
			ObjectID2 = @Object2ID AND
			ObjectRelationTypeID = @ObjectRelationTypeID
			
	RETURN @@ROWCOUNT

END
GO

ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_Object]
GO
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_Object1]
GO
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [FK_Object_Object_Join_ObjectRelationType]
GO
ALTER TABLE [dbo].[Object_Object_Join] DROP CONSTRAINT [DF_Object_Object_Join_DateCreated]
GO
DROP TABLE [dbo].[Object_Object_Join]
GO

CREATE TABLE [dbo].[Object_Object_Join](
	[Object1GUID] [uniqueidentifier] NOT NULL,
	[Object2GUID] [uniqueidentifier] NOT NULL,
	[ObjectRelationTypeID] [int] NOT NULL,
	[Sequence] [int] NULL,
	[DateCreated] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Object_Object_Join] PRIMARY KEY CLUSTERED 
(
	[Object1GUID] ASC,
	[Object2GUID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Object_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Object_Join_Object] FOREIGN KEY([Object1GUID])
REFERENCES [dbo].[Object] ([GUID])
GO

ALTER TABLE [dbo].[Object_Object_Join] CHECK CONSTRAINT [FK_Object_Object_Join_Object]
GO

ALTER TABLE [dbo].[Object_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Object_Join_Object1] FOREIGN KEY([Object2GUID])
REFERENCES [dbo].[Object] ([GUID])
GO

ALTER TABLE [dbo].[Object_Object_Join] CHECK CONSTRAINT [FK_Object_Object_Join_Object1]
GO

ALTER TABLE [dbo].[Object_Object_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Object_Join_ObjectRelationType] FOREIGN KEY([ObjectRelationTypeID])
REFERENCES [dbo].[ObjectRelationType] ([ID])
GO

ALTER TABLE [dbo].[Object_Object_Join] CHECK CONSTRAINT [FK_Object_Object_Join_ObjectRelationType]
GO

ALTER TABLE [dbo].[Object_Object_Join] ADD  CONSTRAINT [DF_Object_Object_Join_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Get Objects
-- =============================================
ALTER PROCEDURE [dbo].[Object_Get]
	@GUIDs					GUIDList Readonly,
	@GroupGUIDs				GUIDList Readonly,
	@UserGUID				uniqueidentifier,
	@IncludeMetadata		bit,
	@IncludeFiles			bit,
	@IncludeObjectRelations	bit,
	@ObjectID				int					= null,
	@ObjectTypeID			int					= null,
	@FolderID				int					= null,
	@PageIndex				int					= 0,
	@PageSize				int					= 10	
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
	  FROM	@PagedResults as p INNER JOIN
				[Object] ON [Object].ID = p.ObjectID
	 
	 if( @IncludeMetadata = 1 )
		SELECT	Metadata.*
		  FROM	@PagedResults as p INNER JOIN
					Metadata ON Metadata.ObjectID = p.ObjectID
		 
	 if( @IncludeFiles = 1 )
		SELECT	[FileInfo].*
		 FROM	@PagedResults as p INNER JOIN
					[FileInfo] ON [FileInfo].ObjectID = p.ObjectID
	
	if( @IncludeObjectRelations = 1 )
		SELECT	DISTINCT Object_Object_Join.*
		  FROM	@PagedResults as p INNER JOIN
					[Object] ON [Object].ID = p.ObjectID INNER JOIN
					Object_Object_Join ON Object_Object_Join.Object1GUID = [Object].[GUID] OR Object_Object_Join.Object2GUID = [Object].[GUID]
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Get Objects
-- =============================================
ALTER PROCEDURE [dbo].[Object_GetAllWithPaging]
	@IncludeMetadata		bit,
	@IncludeFiles			bit,
	@IncludeObjectRelations	bit,
	@ObjectID				int					= null,
	@ObjectTypeID			int					= null,
	@FolderID				int					= null,
	@PageIndex				int					= 0,
	@PageSize				int					= 10
	
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
				( @ObjectTypeID IS NULL OR [Object].ObjectTypeID = @ObjectTypeID )
	)
		
	INSERT INTO	@PagedResults
		 SELECT	* 
		   FROM	ObjectsRN
		  WHERE RowNumber BETWEEN (@PageIndex)     * @PageSize + 1 
							  AND (@PageIndex + 1) * @PageSize

	SELECT	*
	  FROM	@PagedResults as p INNER JOIN
				[Object] ON [Object].ID = p.ObjectID
	 
	 if( @IncludeMetadata = 1 )
		SELECT	Metadata.*
		  FROM	@PagedResults as p INNER JOIN
					Metadata ON Metadata.ObjectID = p.ObjectID
		 
	 if( @IncludeFiles = 1 )
		SELECT	[FileInfo].*
		 FROM	@PagedResults as p INNER JOIN
					[FileInfo] ON [FileInfo].ObjectID = p.ObjectID
					
	if( @IncludeObjectRelations = 1 )
		SELECT	DISTINCT Object_Object_Join.*
		  FROM	@PagedResults as p INNER JOIN
					[Object] ON [Object].ID = p.ObjectID INNER JOIN
					Object_Object_Join ON Object_Object_Join.Object1GUID = [Object].[GUID] OR Object_Object_Join.Object2GUID = [Object].[GUID]

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Get Objects
-- =============================================
ALTER PROCEDURE [dbo].[Object_GetByGUIDs]
	@GUIDs					GUIDList Readonly,
	@IncludeMetadata		bit,
	@IncludeFiles			bit,
	@IncludeObjectRelations	bit
AS
BEGIN

	SET NOCOUNT ON;
		
	SELECT	[Object].*
	  FROM	@GUIDs as g INNER JOIN
				[Object] ON g.GUID = [Object].GUID
	 
	 if( @IncludeMetadata = 1 )
		SELECT	Metadata.*
		  FROM	@GUIDs as g INNER JOIN
					[Object] ON g.GUID = [Object].GUID INNER JOIN
					Metadata ON [Object].ID = Metadata.ObjectID
	
	 if( @IncludeFiles = 1 )
		SELECT	[FileInfo].*
		 FROM	@GUIDs as g INNER JOIN
					[Object] ON g.GUID = [Object].GUID INNER JOIN
					[FileInfo] ON [Object].ID = [FileInfo].ObjectID
					
	if( @IncludeObjectRelations = 1 )
		SELECT	DISTINCT Object_Object_Join.*
		  FROM	@GUIDs as g INNER JOIN
					Object_Object_Join ON Object_Object_Join.Object1GUID = g.[GUID] OR Object_Object_Join.Object2GUID = g.[GUID]

END
GO

ALTER PROCEDURE [dbo].[ObjectRelation_Create]
	@GroupGUIDs				GUIDList Readonly,
	@UserGUID				uniqueidentifier,
	@Object1GUID			uniqueidentifier,
	@Object2GUID			uniqueidentifier,
	@ObjectRelationTypeID	int,
	@Sequence				int	= null
AS
BEGIN

	DECLARE @Object1ID INT
	SELECT	@Object1ID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @Object1GUID

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Metadata', 'CREATE_UPDATE_OBJECTS' )

	IF( @RequiredPermission & dbo.Object_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@Object1ID ) <> @RequiredPermission )
			RETURN -100

	IF EXISTS( SELECT Object1GUID 
	             FROM [Object_Object_Join]
	            WHERE Object1GUID = @Object1GUID AND 
					  Object2GUID = @Object2GUID AND
			          ObjectRelationTypeID = @ObjectRelationTypeID  )
		RETURN -200

	INSERT INTO [Object_Object_Join] (Object1GUID,Object2GUID,[ObjectRelationTypeID],[Sequence],[DateCreated])
		 VALUES (@Object1GUID,@Object2GUID,@ObjectRelationTypeID,@Sequence,GETDATE())
		 
	RETURN @@ROWCOUNT

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.11.28
--				This SP is used to Delete an ObjectRelation
-- =============================================
ALTER PROCEDURE [dbo].[ObjectRelation_Delete]
	@GroupGUIDs				GUIDList Readonly,
	@UserGUID				uniqueidentifier,
	@Object1GUID			uniqueidentifier,
	@Object2GUID			uniqueidentifier,
	@ObjectRelationTypeID	int
AS
BEGIN

	DECLARE @Object1ID INT
	
	SELECT	@Object1ID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @Object1GUID

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Metadata', 'CREATE_UPDATE_OBJECTS' )

	IF( @RequiredPermission & dbo.Object_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@Object1ID ) <> @RequiredPermission )
			RETURN -100

	DELETE
	  FROM	Object_Object_Join
	 WHERE	Object1GUID = @Object1GUID AND 
			Object2GUID = @Object2GUID AND
			ObjectRelationTypeID = @ObjectRelationTypeID
			
	RETURN @@ROWCOUNT

END
GO

