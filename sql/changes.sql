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
		DBCC CHECKIDENT ("[Language]", RESEED,0)
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
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Afrikaans','af-ZA','South Africa')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Albanian','sq-AL','Albania')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-DZ','Algeria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-BH','Bahrain')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-EG','Egypt')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-IQ','Iraq')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-JO','Jordan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-KW','Kuwait')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-LB','Lebanon')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-LY','Libya')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-MA','Morocco')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-OM','Oman')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-QA','Qatar')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-SA','Saudi Arabia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-SY','Syria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-TN','Tunisia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-AE','United Arab Emirates')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Arabic','ar-YE','Yemen')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Armenian','hy-AM','Armenia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Cyrillic','az-AZ-Cyrl','Azerbaijan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Latin','az-AZ-Latn','Azerbaijan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Basque','eu-ES','Basque')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Belarusian','be-BY','Belarus')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Bulgarian','bg-BG','Bulgaria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Catalan','ca-ES','Catalan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-HK','Hong Kong SAR')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-MO','Macau SAR')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-CN','China')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-CHS','Chinese (Simplified)')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-SG','Singapore')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Chinese','zh-TW','Taiwan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Croatian','hr-HR','Croatia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Czech','cs-CZ','Czech Republic')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Danish','da-DK','Denmark')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Dhivehi','div-MV','Maldives')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Dutch','nl-BE','Belgium')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Dutch','nl-NL','The Netherlands')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-AU','Australia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-BZ','Belize')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-CA','Canada')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-CB','Caribbean')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-IE','Ireland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-JM','Jamaica')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-NZ','New Zealand')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-PH','Philippines')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-ZA','South Africa')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-TT','Trinidad and Tobago')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-GB','United Kingdom')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-US','United States')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('English','en-ZW','Zimbabwe')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Estonian','et-EE','Estonia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Faroese','fo-FO','Faroe Islands')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Farsi','fa-IR','Iran')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Finnish','fi-FI','Finland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-BE','Belgium')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-CA','Canada')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-FR','France')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-LU','Luxembourg')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-MC','Monaco')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('French','fr-CH','Switzerland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Galician','gl-ES','Galician')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Georgian','ka-GE','Georgia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-AT','Austria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-DE','Germany')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-LI','Liechtenstein')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-LU','Luxembourg')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('German','de-CH','Switzerland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Greek','el-GR','Greece')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Gujarati','gu-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Hebrew','he-IL','Israel')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Hindi','hi-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Hungarian','hu-HU','Hungary')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Icelandic','is-IS','Iceland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Indonesian','id-ID','Indonesia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Italian','it-IT','Italy')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Italian','it-CH','Switzerland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Japanese','ja-JP','Japan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Kannada','kn-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Kazakh','kk-KZ','Kazakhstan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Konkani','kok-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Korean','ko-KR','Korea')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Kyrgyz','ky-KZ','Kazakhstan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Latvian','lv-LV','Latvia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Lithuanian','lt-LT','Lithuania')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Macedonian','mk-MK','FYROM')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Malay','ms-BN','Brunei')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Malay','ms-MY','Malaysia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Marathi','mr-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Mongolian','mn-MN','Mongolia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Bokml','nb-NO','Norway')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Nynorsk','nn-NO','Norway')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Polish','pl-PL','Polish - Poland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Portuguese','pt-BR','Brazil')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Portuguese','pt-PT','Portugal')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Punjabi','pa-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Romanian','ro-RO','Romania')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Russian','ru-RU','Russia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Sanskrit','sa-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Cyrillic','sr-SP-Cyrl','Serbia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Latin','sr-SP-Latn','Serbia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Slovak','sk-SK','Slovakia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Slovenian','sl-SI','Slovenia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-AR','Argentina')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-BO','Bolivia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-CL','Chile')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-CO','Colombia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-CR','Costa Rica')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-DO','Dominican Republic')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-EC','Ecuador')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-SV','El Salvador')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-GT','Guatemala')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-HN','Honduras')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-MX','Mexico')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-NI','Nicaragua')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-PA','Panama')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-PY','Paraguay')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-PE','Peru')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-PR','Puerto Rico')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-ES','Spain')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-UY','Uruguay')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Spanish','es-VE','Venezuela')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Swahili','sw-KE','Kenya')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Swedish','sv-FI','Finland')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Swedish','sv-SE','Sweden')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Syriac','syr-SY','Syria')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Tamil','ta-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Tatar','tt-RU','Russia')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Telugu','te-IN','India')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Thai','th-TH','Thailand')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Turkish','tr-TR','Turkey')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Ukrainian','uk-UA','Ukraine')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Urdu','ur-PK','Pakistan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Cyrillic','uz-UZ-Cyrl','Uzbekistan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Latin','uz-UZ-Latn','Uzbekistan')
		INSERT INTO [Language]([Name],[LanguageCode],[CountryName])VALUES('Vietnamese','vi-VN','Vietnam')

		INSERT INTO [Metadata]([ObjectID],[LanguageID],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@ObjectID,2,1,'<demo><title>title</title><abstract>abstract</abstract><description>description</description></demo>',GETDATE(),GETDATE(),null,null)

	END
	GO