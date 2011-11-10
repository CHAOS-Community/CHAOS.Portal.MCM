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
GO

DROP TABLE [dbo].[Language]
GO

CREATE TABLE [dbo].[Language](
	[LanguageCode] [varchar](10) NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Language_1] PRIMARY KEY CLUSTERED 
(
	[LanguageCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [MCM]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Metadata_Language]') AND parent_object_id = OBJECT_ID(N'[dbo].[Metadata]'))
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Language]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Metadata_MetadataSchema]') AND parent_object_id = OBJECT_ID(N'[dbo].[Metadata]'))
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_MetadataSchema]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Metadata_Object]') AND parent_object_id = OBJECT_ID(N'[dbo].[Metadata]'))
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Object]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Metadata_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [DF_Metadata_DateCreated]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Metadata_DateModified]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [DF_Metadata_DateModified]
END

GO

USE [MCM]
GO

/****** Object:  Table [dbo].[Metadata]    Script Date: 11/02/2011 15:36:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Metadata]') AND type in (N'U'))
DROP TABLE [dbo].[Metadata]
GO

CREATE TABLE [dbo].[Metadata](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectID] [int] NOT NULL,
	[LanguageCode] [varchar](10) NULL,
	[MetadataSchemaID] [int] NOT NULL,
	[MetadataXml] [xml] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[DateLocked] [datetime] NULL,
	[LockUserGUID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Metadata] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_Language] FOREIGN KEY([LanguageCode])
REFERENCES [dbo].[Language] ([LanguageCode])
GO

ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_Language]
GO

ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_MetadataSchema] FOREIGN KEY([MetadataSchemaID])
REFERENCES [dbo].[MetadataSchema] ([ID])
GO

ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_MetadataSchema]
GO

ALTER TABLE [dbo].[Metadata]  WITH CHECK ADD  CONSTRAINT [FK_Metadata_Object] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[Object] ([ID])
GO

ALTER TABLE [dbo].[Metadata] CHECK CONSTRAINT [FK_Metadata_Object]
GO

ALTER TABLE [dbo].[Metadata] ADD  CONSTRAINT [DF_Metadata_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO

ALTER TABLE [dbo].[Metadata] ADD  CONSTRAINT [DF_Metadata_DateModified]  DEFAULT (getdate()) FOR [DateModified]
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
		INSERT INTO [ObjectType] ([Value])VALUES ('Asset') DECLARE @ObjectTypeID INT SET @ObjectTypeID = @@IDENTITY
		INSERT INTO [ObjectType] ([Value])VALUES ('demo')
		INSERT INTO [ObjectRelationType]([Value])VALUES('Contains')
		INSERT INTO [FolderType]([Name],[DateCreated]) VALUES('TEST',GETDATE())
		INSERT INTO [FolderType]([Name],[DateCreated]) VALUES('Folder',GETDATE()) DECLARE @FolderTypeID INT SET @FolderTypeID = @@IDENTITY		
		INSERT INTO [FormatType]([Value])VALUES('Video')
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
 <xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">

 <xs:element name="demo">
   <xs:complexType>
     <xs:sequence>
       <xs:element name="title" type="xs:string"/>
       <xs:element name="abstract" type="xs:string"/>
       <xs:element name="description" type="xs:string"/>
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

		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@ObjectID,'en',1,'<demo><title>title</title><abstract>abstract</abstract><description>description</description></demo>',GETDATE(),GETDATE(),null,null)

		-----------------------------> INSERT TEST OBJECTS <-------------------------------------------------------------------------------------------------------------------->
		DECLARE @TestOjectID INT
		
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38428',@ObjectTypeID,GETDATE()) SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My second title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)
		
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38429',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 3. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)
		
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38430',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 4. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38431',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 5. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38432',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 6. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38433',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 7. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38434',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 8. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38435',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 9. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38436',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 10. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38437',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 11. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38438',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],[IsShortcut],[DateCreated]) VALUES (@TestOjectID,@TopFolderID,0,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<demo><title>My 12. title</title><abstract>This is a short descripton about the content</abstract><description>The long long long and exiting description about the content</description></demo>',GETDATE(),GETDATE(),null,null)

		

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
				( o.[GUID] in ( SELECT g.[GUID] FROM @GUIDs as g ) ) AND
				( @ObjectID IS NULL OR o.ID = @ObjectID ) AND
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
GO 

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.10.13
--				This SP is used to get metadatas
-- =============================================
ALTER PROCEDURE [dbo].[Metadata_Get]
	@ObjectGUID			uniqueidentifier,
	@MetadataSchemaGUID uniqueidentifier = NULL,
	@LanguageCode		varchar(10)      = NULL
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
			( @LanguageCode IS NULL OR @LanguageCode = Metadata.LanguageCode )
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP Creates a language
-- =============================================
ALTER PROCEDURE [dbo].[Language_Create]
	@Name				varchar(255),
	@LanguageCode		varchar(10),
	@SystemPermission	int
AS
BEGIN
	
	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100
	
	INSERT INTO [Language] ([Name],[LanguageCode])
         VALUES	(@Name, @LanguageCode)

	RETURN @@IDENTITY

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP is used to get Languages
-- =============================================
ALTER PROCEDURE [dbo].[Language_Get]
	@Name			varchar(255)	= null,
	@LanguageCode	varchar(10)		= null
AS
BEGIN

	SET NOCOUNT ON;

    SELECT	*
      FROM	[Language]
     WHERE	(@Name IS NULL OR Name = @Name) AND
			(@LanguageCode IS NULL OR LanguageCode = @LanguageCode)
	
END
GO
-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP Updates a language
-- =============================================
ALTER PROCEDURE [dbo].[Language_Update]
	@Name				varchar(255),
	@LanguageCode		varchar(10),
	@SystemPermission	int
AS
BEGIN
	
	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100
	
	UPDATE	[Language]
	   SET	[Name] = @Name
	 WHERE	LanguageCode = @LanguageCode

	RETURN @@ROWCOUNT

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.23
--				This SP Deletes a language
-- =============================================
ALTER PROCEDURE [dbo].[Language_Delete]
	@LanguageCode		varchar(10),
	@SystemPermission	int
AS
BEGIN
	
	DECLARE @RequiredPermission INT
	SET @RequiredPermission = dbo.GetPermissionForAction( 'System', 'Manage Type' )

	IF( @RequiredPermission & @SystemPermission <> @RequiredPermission )
		RETURN -100
	
	DELETE	[Language]
	 WHERE	LanguageCode = @LanguageCode

	RETURN @@ROWCOUNT

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
-- Create date: 2011.11.04
--				This Table function returns the parent folder chain
-- =============================================
CREATE FUNCTION GetParentFolders
(
	@FolderID int
)
RETURNS 
@Folders TABLE
(
	FolderID int
)
AS
BEGIN
	DECLARE @ParentID INT

	SELECT	@ParentID = ParentID
	  FROM	Folder
	 WHERE	ID = @FolderID

	INSERT INTO @Folders VALUES (@FolderID)

	WHILE( @ParentID IS NOT NULL )
	BEGIN

		INSERT INTO @Folders VALUES(@ParentID)

		SELECT	@ParentID = ParentID 
		  FROM	Folder
		 WHERE  ID = @ParentID

	END
	
	RETURN
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.11.04
--				This SP return folders associated with an Object and the option of returning all parent folders too
-- =============================================
CREATE PROCEDURE Folder_Get
	@ObjectID			int,
	@IncludeFolderTree	bit
AS
BEGIN

	SET NOCOUNT ON;

	IF( @IncludeFolderTree = 1 )
	BEGIN
	
		DECLARE @Folders TABLE
		(
			FolderID int
		)

		DECLARE @FolderID INT	 
		DECLARE Iterator CURSOR
			FOR SELECT	[Folder].[ID]
				  FROM	[Folder] INNER JOIN Object_Folder_Join ON
								 Folder.ID = Object_Folder_Join.FolderID
				 WHERE	Object_Folder_Join.ObjectID = @ObjectID
			
			OPEN Iterator
			
			FETCH NEXT FROM Iterator
				INTO @FolderID
			
		WHILE( @@FETCH_STATUS = 0 )
		BEGIN

			INSERT INTO @Folders
				 SELECT FolderID FROM dbo.GetParentFolders( @FolderID )
			     
			FETCH NEXT FROM Iterator
				INTO @FolderID
		END
			
		SELECT	[Folder].[ID]
		  FROM	[Folder]
		 WHERE	ID IN (SELECT FolderID FROM @Folders)
		 
		 CLOSE Iterator;
		 DEALLOCATE Iterator;
	
	END
	ELSE
	BEGIN
	
		SELECT	[Folder].[ID]
		  FROM	[Folder] INNER JOIN Object_Folder_Join ON
						 Folder.ID = Object_Folder_Join.FolderID
		 WHERE	Object_Folder_Join.ObjectID = @ObjectID

	END
	
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.10.11
--				This SP is used to get folders by direct association from user and groups
-- =============================================
ALTER PROCEDURE [dbo].[Folder_Get_DirectFolderAssociations]
	@GroupGUIDs			GUIDList Readonly,
	@UserGUID			uniqueidentifier,
	@RequiredPermission	int
AS
BEGIN

	SET NOCOUNT ON;

    SELECT [Folder].*
      FROM [Folder] 
            LEFT OUTER JOIN [Folder_User_Join]  ON [Folder].ID = [Folder_User_Join].FolderID
            LEFT OUTER JOIN [Folder_Group_Join] ON [Folder].ID = [Folder_Group_Join].FolderID
     WHERE	( [Folder_User_Join].UserGUID = @UserGUID OR 
              [Folder_Group_Join].GroupGUID IN ( SELECT [GUID] FROM @GroupGUIDs ) ) AND
			( [Folder_User_Join].Permission  & @RequiredPermission = @RequiredPermission OR
			  [Folder_Group_Join].Permission & @RequiredPermission = @RequiredPermission )
	
	-- Test which is faster
    --SELECT [Folder].*
    --  FROM [Folder] 
    --        INNER JOIN [Folder_User_Join]  ON [Folder].ID = [Folder_User_Join].FolderID
    -- WHERE [Folder_User_Join].UserGUID = @UserGUID AND
    --       [Folder_User_Join].Permission & @RequiredPermission = @RequiredPermission
    --UNION ALL
    --SELECT [Folder].*
    --  FROM [Folder] 
    --        INNER JOIN [Folder_Group_Join] ON [Folder].ID = [Folder_Group_Join].FolderID
    -- WHERE [Folder_Group_Join].GroupGUID IN ( SELECT [GUID] FROM @GroupGUIDs ) AND
    --       [Folder_Group_Join].Permission & @RequiredPermission = @RequiredPermission
     

END
