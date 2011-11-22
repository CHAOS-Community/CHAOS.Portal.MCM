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
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Get Objects
-- =============================================
CREATE PROCEDURE [dbo].[Object_GetAllWithPaging]
	@IncludeMetadata	bit,
	@IncludeFiles		bit,
	@ObjectID			int					= null,
	@ObjectTypeID		int					= null,
	@FolderID			int					= null,
	@PageIndex			int					= 0,
	@PageSize			int					= 10
	
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
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.11.21
--				This SP is used to create and associate a file with an object
-- =============================================
CREATE PROCEDURE File_Create
	@GroupGUIDs			GUIDList Readonly,
	@UserGUID			uniqueidentifier,
	@ObjectGUID			uniqueidentifier,
	@ParentFileID		int					= null,
	@FormatID			int,
	@DestinationID		int,
	@Filename			varchar(MAX),
	@OriginalFilename	varchar(MAX),
	@FolderPath			varchar(MAX)
AS
BEGIN
	
	DECLARE @ObjectID INT
	
	SELECT	@ObjectID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @ObjectGUID
	
	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'CREATE_UPDATE_OBJECTS' )

	IF( @RequiredPermission & dbo.Object_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@ObjectID ) <> @RequiredPermission )
			RETURN -100
	
	INSERT INTO [File] ([ObjectID],[ParentID],[FormatID],[DestinationID],[Filename],[OriginalFilename],[FolderPath],[DateCreated])
		 VALUES (@ObjectID,@ParentFileID,@FormatID,@DestinationID,@Filename,@OriginalFilename,@FolderPath,GETDATE())

	RETURN @@IDENTITY
	
END
GO

ALTER TABLE Format DROP COLUMN FileExtension
GO
	
ALTER TABLE AccessProvider ADD Token varchar(255) NOT NULL
GO

CREATE VIEW [dbo].[FileInfo]
AS
SELECT     dbo.[File].ID, dbo.[File].ParentID, dbo.[File].ObjectID, dbo.[File].Filename, dbo.[File].OriginalFilename, dbo.AccessProvider.Token, 
                      REPLACE(REPLACE(REPLACE(dbo.AccessProvider.StringFormat, '{BASE_PATH}', dbo.AccessProvider.BasePath), '{FOLDER_PATH}', dbo.[File].FolderPath), 
                      '{FILENAME}', dbo.[File].Filename) AS URL, dbo.[File].FormatID, dbo.Format.Name AS Format, dbo.FormatCategory.Value AS FormatCategory, 
                      dbo.FormatType.Value AS FormatType
FROM         dbo.[File] INNER JOIN
                      dbo.AccessProvider ON dbo.AccessProvider.DestinationID = dbo.[File].DestinationID INNER JOIN
                      dbo.Format ON dbo.[File].FormatID = dbo.Format.ID INNER JOIN
                      dbo.FormatCategory ON dbo.Format.FormatCategoryID = dbo.FormatCategory.ID INNER JOIN
                      dbo.FormatType ON dbo.FormatCategory.FormatTypeID = dbo.FormatType.ID

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

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Get Objects
-- =============================================
ALTER PROCEDURE [dbo].[Object_GetAllWithPaging]
	@IncludeMetadata	bit,
	@IncludeFiles		bit,
	@ObjectID			int					= null,
	@ObjectTypeID		int					= null,
	@FolderID			int					= null,
	@PageIndex			int					= 0,
	@PageSize			int					= 10
	
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

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Get Objects
-- =============================================
ALTER PROCEDURE [dbo].[Object_GetByGUIDs]
	@GUIDs				GUIDList Readonly,
	@IncludeMetadata	bit,
	@IncludeFiles		bit
	
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

END
GO

CREATE STATISTICS [_dta_stat_741577680_4_5_2] ON [dbo].[File]([FormatID], [DestinationID], [ObjectID])
GO
CREATE NONCLUSTERED INDEX [_dta_index_File_5_741577680__K2_K5_K4_1_3_6_7_8] ON [dbo].[File] 
(
	[ObjectID] ASC,
	[DestinationID] ASC,
	[FormatID] ASC
)
INCLUDE ( [ID],
[ParentID],
[Filename],
[OriginalFilename],
[FolderPath]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

-- =============================================
-- Author:		Jesper Fyhr	Knudsen
-- Create date: 2010.08.17
--				This SP Pupulate MCM with Default data
-- =============================================
ALTER PROCEDURE [dbo].[PopulateDefaultData]

AS
	IF( 1 = 1 )
	BEGIN

		DELETE FROM AccessPoint_User_Join
		DELETE FROM AccessPoint_Group_Join
		DELETE FROM AccessPoint_Object_Join
		DELETE FROM AccessPoint
		DELETE FROM AccessProvider
		DELETE FROM Destination
		DELETE FROM [File]
		DELETE FROM Object_Folder_Join
		DELETE FROM Folder_Group_Join
		DELETE FROM Folder_User_Join
		DELETE FROM Folder
		DELETE FROM FolderType
		DELETE FROM Format
		DELETE FROM FormatCategory
		DELETE FROM FormatType
		DELETE FROM Metadata
		DELETE FROM MetadataSchema_Group_Join
		DELETE FROM MetadataSchema_User_Join
		DELETE FROM MetadataSchema
		DELETE FROM Object_Object_Join
		DELETE FROM [Object]
		DELETE FROM ObjectType
		DELETE FROM Permission
		DELETE FROM ObjectRelationType
		DELETE FROM [Language]

		DBCC CHECKIDENT ("AccessPoint", RESEED,0)
		DBCC CHECKIDENT ("AccessProvider", RESEED,0)
		DBCC CHECKIDENT ("[File]", RESEED,0)
		DBCC CHECKIDENT ("FolderType", RESEED,0)
		DBCC CHECKIDENT ("Folder", RESEED,0)
		DBCC CHECKIDENT ("Format", RESEED,0)
		DBCC CHECKIDENT ("FormatCategory", RESEED,0)
		DBCC CHECKIDENT ("FormatType", RESEED,0)
		DBCC CHECKIDENT ("Metadata", RESEED,0)
		DBCC CHECKIDENT ("MetadataSchema", RESEED,0)
		DBCC CHECKIDENT ("Object", RESEED,0)
		DBCC CHECKIDENT ("ObjectType", RESEED,0)
		DBCC CHECKIDENT ("ObjectRelationType", RESEED,0)

		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('System','Manage Type',4,'Permissoin to manage Types')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','GET',1,'Permissoin to GET Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','DELETE',2,'Permissoin to DELETE Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','UPDATE',4,'Permissoin to UPDATE Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','MOVE_FOLDER',8,'Permissoin to MOVE Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','CREATE',16,'Permissoin to Create Folders')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','GET_OBJECTS',32,'Permissoin to GET Objects in folder')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','CREATE_UPDATE_OBJECTS',64,'Permissoin to CREATE / UPDATE or MOVE Objects to/in a folder')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','DELETE_OBJECTS',128,'Permissoin to DELETE Objects in folder')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','MOVE_OBJECT_FROM',256,'Permissoin to MOVE Objects from the folder')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Subscription','CREATE TOPFOLDER',16,'Permissoin to Create top folders')
		INSERT INTO [Destination]([ID],[SubscriptionGUID],[Title],[DateCreated]) VALUES (1,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','DMB Source',GETDATE())
		INSERT INTO [Destination]([ID],[SubscriptionGUID],[Title],[DateCreated]) VALUES (2,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','DMB Thumbnail',GETDATE())
		INSERT INTO [AccessProvider]([DestinationID],[BasePath],[StringFormat],[DateCreated],[Token])VALUES(1,'\\TRANSJOB03\Asset\Files','{BASE_PATH}{FOLDER_PATH}{FILENAME}',GETDATE(),'Windows UNC')
		INSERT INTO [AccessProvider]([DestinationID],[BasePath],[StringFormat],[DateCreated],[Token])VALUES(2,'\\TRANSJOB03\Asset\Files','{BASE_PATH}{FOLDER_PATH}{FILENAME}',GETDATE(),'Windows UNC')
		INSERT INTO [ObjectType] ([Value])VALUES ('Asset') DECLARE @ObjectTypeID INT SET @ObjectTypeID = @@IDENTITY
		INSERT INTO [ObjectType] ([Value])VALUES ('demo')
		INSERT INTO [ObjectRelationType]([Value])VALUES('Contains')
		INSERT INTO [FolderType]([Name],[DateCreated]) VALUES('TEST',GETDATE())
		INSERT INTO [FolderType]([Name],[DateCreated]) VALUES('Folder',GETDATE()) DECLARE @FolderTypeID INT SET @FolderTypeID = @@IDENTITY		
		INSERT INTO [FormatType]([Value])VALUES('Video') DECLARE @VideoFormatType INT SET @VideoFormatType = @@IDENTITY
		INSERT INTO [FormatType]([Value])VALUES('Audio') DECLARE @AudioFormatType INT SET @AudioFormatType = @@IDENTITY
		INSERT INTO [FormatType]([Value])VALUES('Image') DECLARE @ImageFormatType INT SET @ImageFormatType = @@IDENTITY
		INSERT INTO [FormatType]([Value])VALUES('Other') DECLARE @OtherFormatType INT SET @OtherFormatType = @@IDENTITY
		INSERT INTO [FormatCategory] ([FormatTypeID] ,[Value]) VALUES(@VideoFormatType,'Video Source') DECLARE @VideoSourceFormatCategory INT SET @VideoSourceFormatCategory = @@IDENTITY
		INSERT INTO [FormatCategory] ([FormatTypeID] ,[Value]) VALUES(@AudioFormatType,'Audio Source') DECLARE @AudioSourceFormatCategory INT SET @AudioSourceFormatCategory = @@IDENTITY
		INSERT INTO [FormatCategory] ([FormatTypeID] ,[Value]) VALUES(@ImageFormatType,'Image Source') DECLARE @ImageSourceFormatCategory INT SET @ImageSourceFormatCategory = @@IDENTITY
		INSERT INTO [FormatCategory] ([FormatTypeID] ,[Value]) VALUES(@OtherFormatType,'Other Source') DECLARE @OtherSourceFormatCategory INT SET @OtherSourceFormatCategory = @@IDENTITY
		INSERT INTO [FormatCategory] ([FormatTypeID] ,[Value]) VALUES(@ImageFormatType,'Thumbnail')    DECLARE @ThumbnailFormatCategory   INT SET @ThumbnailFormatCategory   = @@IDENTITY
		INSERT INTO [Format]([FormatCategoryID],[Name],[FormatXml],[MimeType]) VALUES (@VideoSourceFormatCategory,'Unknown video format',null,'application/octet-stream')
		INSERT INTO [Format]([FormatCategoryID],[Name],[FormatXml],[MimeType]) VALUES (@AudioSourceFormatCategory,'Unknown audio format',null,'application/octet-stream')
		INSERT INTO [Format]([FormatCategoryID],[Name],[FormatXml],[MimeType]) VALUES (@ImageSourceFormatCategory,'Unknown image format',null,'application/octet-stream')
		INSERT INTO [Format]([FormatCategoryID],[Name],[FormatXml],[MimeType]) VALUES (@OtherSourceFormatCategory,'Unknown format',null,'application/octet-stream')
		INSERT INTO [Format]([FormatCategoryID],[Name],[FormatXml],[MimeType]) VALUES (@ThumbnailFormatCategory,'JPEG 256x256> q90',null,'image/jpeg')
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(null,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Geckon',GETDATE()) DECLARE @TopFolderID INT SET @TopFolderID = @@IDENTITY
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@TopFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Public',GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@TopFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Users',GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@@IDENTITY,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Private',GETDATE()) DECLARE @PrivateFolder INT SET @PrivateFolder = @@IDENTITY
		INSERT INTO [Folder_Group_Join]([FolderID],[GroupGUID],[Permission],[DateCreated])VALUES(@PrivateFolder,'A0B231E9-7D98-4F52-885E-AAAAAAAAAAAA',  0x00000001,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@PrivateFolder,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','sub',GETDATE())
		INSERT INTO [Folder_Group_Join]([FolderID],[GroupGUID],[Permission],[DateCreated])VALUES(@TopFolderID,'A0B231E9-7D98-4F52-885E-AAAAAAAAAAAA',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(null,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Test',GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@@IDENTITY,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','sub Test',GETDATE())
		INSERT INTO [Folder_Group_Join]([FolderID],[GroupGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'A0B231E9-7D98-4F52-885E-AAAAAAAAAAAA',  0x0000000F,GETDATE())
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38427',@ObjectTypeID,GETDATE()) DECLARE @ObjectID INT SET @ObjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@ObjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [MetadataSchema]([GUID],[name],[SchemaXml],[DateCreated]) VALUES('37B0E892-3943-41A9-8322-241D6277E528','demo','<?xml version="1.0"?>
<xs:schema attributeFormDefault="unqualified" elementFormDefault="qualified" xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="xml">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="ID" type="xs:int" />
        <xs:element name="Name" type="xs:string" />
        <xs:element name="ShortDescription" type="xs:string" />
        <xs:element name="Abstract" type="xs:string" />
        <xs:element name="CreateDate" type="xs:string" />
        <xs:element name="UpdateDate" type="xs:string" />
        <xs:element name="TechnicalComment" type="xs:string" />
        <xs:element name="Subjects">
          <xs:complexType>
            <xs:sequence>
              <xs:element maxOccurs="unbounded" name="Subject" type="xs:string" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="Length" type="xs:time" />
        <xs:element name="Locations">
          <xs:complexType>
            <xs:sequence>
              <xs:element maxOccurs="unbounded" name="Location" type="xs:string" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="Colophon" type="xs:string" />
        <xs:element name="Actors">
          <xs:complexType>
            <xs:sequence>
              <xs:element maxOccurs="unbounded" name="Actor">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name="Name" type="xs:string" />
                    <xs:element name="Role" type="xs:string" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name="OriginalDate" type="xs:string" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>',GETDATE())
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Afrikaans','af')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Albanian','sq')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Arabic','ar')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Armenian','hy')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Basque','eu')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Belarusian','be')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Bulgarian','bg')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Catalan','ca')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Chinese','zh')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Croatian','hr')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Czech','cs')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Danish','da')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Dhivehi','div')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Dutch','nl')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('English','en')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Estonian','et')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Faroese','fo')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Farsi','fa')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Finnish','fi')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('French','fr')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Galician','gl')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Georgian','ka')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('German','de')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Greek','el')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Gujarati','gu')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Hebrew','he')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Hindi','hi')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Hungarian','hu')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Icelandic','is')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Indonesian','id')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Italian','it')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Japanese','ja')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Kannada','kn')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Kazakh','kk')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Konkani','kok')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Korean','ko')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Kyrgyz','ky')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Latvian','lv')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Lithuanian','lt')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Macedonian','mk')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Malay','ms')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Marathi','mr')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Mongolian','mn')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Bokml','nb')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Nynorsk','nn')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Polish','pl')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Portuguese','pt')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Punjabi','pa')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Romanian','ro')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Russian','ru')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Sanskrit','sa')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Slovak','sk')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Slovenian','sl')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Spanish','es')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Swahili','sw')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Swedish','sv')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Syriac','syr')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Tamil','ta')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Tatar','tt')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Telugu','te')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Thai','th')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Turkish','tr')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Ukrainian','uk')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Urdu','ur')
		INSERT INTO [Language]([Name],[LanguageCode])VALUES('Vietnamese','vi')

		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@ObjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		-----------------------------> INSERT TEST OBJECTS <-------------------------------------------------------------------------------------------------------------------->
		DECLARE @TestOjectID INT
		
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38428',@ObjectTypeID,GETDATE()) SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)
		
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38429',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)
		
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38430',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38431',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38432',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38433',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38434',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38435',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38436',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38437',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38438',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

	END
