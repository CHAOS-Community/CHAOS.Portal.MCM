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
		SELECT	ROW_NUMBER() OVER(ORDER BY [Object].DateCreated) AS RowNumber, 
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
-- Create date: 2012.01.06
--				This SP is used to get files
-- =============================================
CREATE PROCEDURE File_Get
	@FileID		int
AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT	*
      FROM	[File]
     WHERE	[File].ID = @FileID
     
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2012.01.06
--				This SP is used to get destinations
-- =============================================
CREATE PROCEDURE DestinationInfo_Get
	@DestinationID		int
AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT	*
      FROM	DestinationInfo
     WHERE	DestinationInfo.ID = @DestinationID
     
END
GO

CREATE VIEW [dbo].[DestinationInfo]
AS
SELECT	dbo.Destination.ID, 
		dbo.Destination.SubscriptionGUID, 
		dbo.Destination.Title, 
		dbo.Destination.DateCreated, 
		dbo.AccessProvider.BasePath, 
        dbo.AccessProvider.StringFormat, 
        dbo.AccessProvider.Token
FROM    dbo.Destination 
			INNER JOIN dbo.AccessProvider ON dbo.Destination.ID = dbo.AccessProvider.DestinationID

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
			
	-- Delete object, this should be changed when links are implemented
	IF NOT EXISTS( SELECT * FROM Object_Folder_Join WHERE ObjectID = @ObjectID )
	BEGIN
		DELETE 
		  FROM	[Metadata]
		 WHERE	ObjectID = @ObjectID
	
		DELETE
		  FROM	[Object]
		 WHERE	ID = @ObjectID
	END
	
	IF( @@ERROR <> 0 )
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
	
	RETURN 1
	
END
GO

ALTER TABLE Object_Folder_Join ALTER COLUMN IsShortcut int not null
EXEC sp_rename 'Object_Folder_Join.[IsShortcut]', 'ObjectFolderTypeID', 'COLUMN'
GO

UPDATE	Object_Folder_Join
   SET	Object_Folder_Join.ObjectFolderTypeID = 1
   
CREATE TABLE [dbo].[ObjectFolderType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ObjectFolderType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [ObjectFolderType] ([ID] ,[Name]) VALUES (1,'Location')
GO

ALTER TABLE [dbo].[Object_Folder_Join]  WITH CHECK ADD  CONSTRAINT [FK_Object_Folder_Join_ObjectFolderType] FOREIGN KEY([ObjectFolderTypeID])
REFERENCES [dbo].[ObjectFolderType] ([ID])
GO

ALTER TABLE [dbo].[Object_Folder_Join] CHECK CONSTRAINT [FK_Object_Folder_Join_ObjectFolderType]
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

		DELETE FROM [File]
		DELETE FROM AccessProvider
		DELETE FROM Destination
		DELETE FROM Format
		DELETE FROM FormatCategory
		DELETE FROM FormatType
		DELETE FROM AccessPoint_User_Join
		DELETE FROM AccessPoint_Group_Join
		DELETE FROM AccessPoint_Object_Join
		DELETE FROM AccessPoint
		DELETE FROM Object_Folder_Join
		DELETE FROM [ObjectFolderType]
		DELETE FROM Folder_Group_Join
		DELETE FROM Folder_User_Join
		DELETE FROM Folder
		DELETE FROM FolderType
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
		DBCC CHECKIDENT ("[ObjectFolderType]", RESEED,0)
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

		INSERT INTO [ObjectFolderType] ([Name]) VALUES ('Location')
		INSERT INTO [ObjectFolderType] ([Name]) VALUES ('Link')
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
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','LINK_OBJECT_FROM',512,'Permissoin to LINK Objects from the folder')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Folder','LINK_OBJECT_TO',1024,'Permissoin to LINK Objects to the folder')
		INSERT INTO [Permission]([TableIdentifier],[RightName],[Permission],[Description]) VALUES ('Subscription','CREATE TOPFOLDER',16,'Permissoin to Create top folders')
		INSERT INTO [Destination]([ID],[SubscriptionGUID],[Title],[DateCreated]) VALUES (1,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','DMB Source',GETDATE())
		INSERT INTO [Destination]([ID],[SubscriptionGUID],[Title],[DateCreated]) VALUES (2,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','DMB Thumbnail',GETDATE())
		INSERT INTO [Destination]([ID],[SubscriptionGUID],[Title],[DateCreated]) VALUES (3,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Bonanza Thumbnail',GETDATE())
		INSERT INTO [Destination]([ID],[SubscriptionGUID],[Title],[DateCreated]) VALUES (4,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','LARM Thumbnail',GETDATE())
		INSERT INTO [AccessProvider] ([DestinationID],[BasePath],[StringFormat],[DateCreated],[Token])VALUES (1,'\\TRANSJOB03\Asset\Files','{BASE_PATH}{FOLDER_PATH}{FILENAME}',GETDATE(),'Windows UNC')
		INSERT INTO [AccessProvider] ([DestinationID],[BasePath],[StringFormat],[DateCreated],[Token])VALUES (2,'http://jpeg.dril.dk/DRIL/JPEG','{BASE_PATH}{FOLDER_PATH}{FILENAME}',GETDATE(),'HTTP Download')
		INSERT INTO [AccessProvider] ([DestinationID],[BasePath],[StringFormat],[DateCreated],[Token])VALUES (3,'http://downol.dr.dk/download/bonanza/stills','{BASE_PATH}{FOLDER_PATH}{FILENAME}',GETDATE(),'HTTP Download')
		INSERT INTO [AccessProvider] ([DestinationID],[BasePath],[StringFormat],[DateCreated],[Token])VALUES (3,'\\Datagrp_5\dr$\Online\Download\Bonanza\Stills','{BASE_PATH}{FOLDER_PATH}{FILENAME}',GETDATE(),'Windows UNC')
		INSERT INTO [AccessProvider] ([DestinationID],[BasePath],[StringFormat],[DateCreated],[Token])VALUES (4,'https://s3-eu-west-1.amazonaws.com','{BASE_PATH}{FOLDER_PATH}{FILENAME}',GETDATE(),'HTTP Download')
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
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@TopFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Public',GETDATE()) DECLARE @PublicFolderID INT SET @PublicFolderID = @@IDENTITY
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@TopFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Private',GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@TopFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','Users',GETDATE()) DECLARE @UserFolderID INT SET @UserFolderID = @@IDENTITY
		INSERT INTO [Folder_Group_Join]([FolderID],[GroupGUID],[Permission],[DateCreated])VALUES(@TopFolderID,'A0B231E9-7D98-4F52-885E-AAAAAAAAAAAA',0x7FFFFFFF,GETDATE())
		
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','A0B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'A0B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','B0B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'B0B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','E0B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'E0B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','D0B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'D0B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','F0B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'F0B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','F1B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'F1B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','F2B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'F2B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','F3B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'F3B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','F4B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'F4B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','F5B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'F5B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','F6B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'F6B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','F7B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'F7B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		INSERT INTO [Folder]([ParentID],[FolderTypeID],[SubscriptionGUID],[Title],[DateCreated]) VALUES(@UserFolderID,@FolderTypeID,'9C4E8A99-A69B-41FD-B1C7-E28C54D1D304','F8B231E9-7D98-4F52-885E-AF4837FAA352',GETDATE())
		INSERT INTO [Folder_User_Join]([FolderID],[UserGUID],[Permission],[DateCreated])VALUES(@@IDENTITY,'F8B231E9-7D98-4F52-885E-AF4837FAA352',0x7FFFFFFF,GETDATE())
		
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
INSERT INTO [MetadataSchema]([GUID],[name],[SchemaXml],[DateCreated]) VALUES ('C6CD3DA2-021B-4F42-893D-40ECC4A64DFF','Larm FileInfo','<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" attributeFormDefault="unqualified" elementFormDefault="qualified">
  <xs:element name="Larm.FileInfos">
    <xs:complexType>
      <xs:sequence>
        <xs:element maxOccurs="unbounded" name="FileInfo">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="StartTime" type="xs:dateTime" />
              <xs:element name="EndTime" type="xs:dateTime" />
              <xs:element name="StartOffSet" type="xs:int" />
              <xs:element name="EndOffSet" type="xs:int" />
              <xs:element name="FileName" type="xs:string" />
              <xs:element name="ObjectID" type="xs:double" />
              <xs:element name="Index" type="xs:int" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>',GETDATE())

INSERT INTO [MetadataSchema]([GUID],[name],[SchemaXml],[DateCreated]) VALUES ('CEBF6861-6904-4913-BFFD-EC764F48499D','Larm Persons','<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" attributeFormDefault="unqualified" elementFormDefault="qualified">
  <xs:element name="Larm.Persons">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="PersonInfo">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="Name" type="xs:string" />
              <xs:element name="RoleName" type="xs:string" />
              <xs:element name="Type" type="xs:int" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>',GETDATE())

INSERT INTO [MetadataSchema]([GUID],[name],[SchemaXml],[DateCreated]) VALUES ('42A0B635-E327-44D4-AED3-39B57D15A6D1','Larm Program','<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" attributeFormDefault="unqualified" elementFormDefault="qualified">
  <xs:element name="Larm.Program">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="StartTime" type="xs:dateTime">
          <xs:annotation solrIsField="true" solrFreetextIndex="false" />
        </xs:element>
        <xs:element name="EndTime" type="xs:dateTime">
          <xs:annotation solrIsField="true" solrFreetextIndex="false" />
        </xs:element>
        <xs:element name="ChannelName" type="xs:string">
          <xs:annotation solrIsField="true" />
        </xs:element>
        <xs:element name="Title" type="xs:string">
          <xs:annotation solrIsField="true" />
        </xs:element>
        <xs:element name="Abstract" type="xs:string" />
        <xs:element name="Description" type="xs:string" />
        <xs:element name="Identifier" type="xs:unsignedLong" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
',GETDATE())

INSERT INTO [MetadataSchema]([GUID],[name],[SchemaXml],[DateCreated]) VALUES ('B4325524-5E7E-4A6D-89BD-1BC7F57E98CA','Larm Comment','<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" attributeFormDefault="unqualified" elementFormDefault="qualified">
  <xs:element name="Larm.CommentInfos">
    <xs:complexType>
      <xs:sequence>
        <xs:element maxOccurs="unbounded" name="CommentInfo">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="UserID" type="xs:int">
                <xs:annotation solrFreetextIndex="false" />
              </xs:element>
              <xs:element name="CreateDate" type="xs:dateTime">
                <xs:annotation solrFreetextIndex="false" />
              </xs:element>
              <xs:element name="Title" type="xs:string" />
              <xs:element name="Description" type="xs:string">
                <xs:annotation solrIsField="true" />
              </xs:element>
              <xs:element name="StartTime" type="xs:int">
                <xs:annotation solrFreetextIndex="false" />
              </xs:element>
              <xs:element name="EndTime" type="xs:int">
                <xs:annotation solrFreetextIndex="false" />
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>',GETDATE())

INSERT INTO [MetadataSchema]([GUID],[name],[SchemaXml],[DateCreated]) VALUES ('72E0E593-0C41-4A81-BA36-C41549E95B7A','Larm Playlist','<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema" attributeFormDefault="unqualified" elementFormDefault="qualified">
  <xs:element name="Playlist">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="ID" type="xs:unsignedByte" />
        <xs:element name="Title" type="xs:string" />
        <xs:element name="Description" />
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

		-----------------------------> INSERT TEST OBJECTS <-------------------------------------------------------------------------------------------------------------------->
		DECLARE @TestOjectID INT
		
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38428',@ObjectTypeID,GETDATE()) SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)
		
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38429',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)
		
		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38430',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38431',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38432',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38433',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38434',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38435',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38436',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38437',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

		INSERT INTO [Object] ([GUID],[ObjectTypeID],[DateCreated]) VALUES ('0876EBF6-E30F-4A43-9B6E-F8A479F38438',@ObjectTypeID,GETDATE())SET  @TestOjectID = @@IDENTITY
		INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated]) VALUES (@TestOjectID,@PublicFolderID,1,GETDATE()) 
		INSERT INTO [Metadata]([ObjectID],[LanguageCode],[MetadataSchemaID],[MetadataXml],[DateCreated],[DateModified],[DateLocked],[LockUserGUID])VALUES(@TestOjectID,'en',1,'<xml><ID>51031</ID><Name>Klip af Deadline 18.04.2007. Kl. 22:30</Name><ShortDescription /><Abstract /><CreateDate>16-08-2007 14:16:00</CreateDate><UpdateDate>16-08-2007 14:16:00</UpdateDate><TechnicalComment /><Subjects /><Length /><Locations /><Colophon /><Actors /><OriginalDate>18-04-2007 00:00:00</OriginalDate></xml>',GETDATE(),GETDATE(),null,null)

	END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.16
--				This SP is used to Create Objects
-- =============================================
ALTER PROCEDURE [dbo].[Object_Create]
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
		 
	INSERT INTO [Object_Folder_Join]([ObjectID],[FolderID],ObjectFolderTypeID,[DateCreated])
		 VALUES (@ObjectID,@FolderID,1,GETDATE())
		 
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
			ObjectID = @ObjectID AND
			ObjectFolderTypeID <> 1 -- only delete if it's not the actual location
			
	-- Delete object, this should be changed when links are implemented
	--IF NOT EXISTS( SELECT * FROM Object_Folder_Join WHERE ObjectID = @ObjectID )
	--BEGIN
	--	DELETE 
	--	  FROM	[Metadata]
	--	 WHERE	ObjectID = @ObjectID
	
	--	DELETE
	--	  FROM	[Object]
	--	 WHERE	ID = @ObjectID
	--END
	
	IF( @@ERROR <> 0 )
		ROLLBACK TRANSACTION
	ELSE
		COMMIT TRANSACTION
	
	RETURN 1
	
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2012.01.17
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ObjectFolder_Create
	@GroupGUIDs		GUIDList Readonly,
	@UserGuid		uniqueidentifier,
	@ObjectGUID		uniqueidentifier,
	@FolderID		int,
	@ObjectFolderID	int
AS
BEGIN
	
	IF( @ObjectFolderID = 1 )
		RETURN -100 -- not permitted
	
	DECLARE @ObjectID INT
	SELECT	@ObjectID = ID
	  FROM	[Object]
	 WHERE	[GUID] = @ObjectGUID
	 
	DECLARE	@RequiredPermissionOnObject INT = dbo.GetPermissionForAction( 'Folder', 'LINK_OBJECT_FROM' );
	DECLARE	@RequiredPermissionOnDestinationFolder INT = dbo.GetPermissionForAction( 'Folder', 'LINK_OBJECT_TO' );
	
	IF( dbo.[Object_FindHighestUserPermission](@UserGuid,@GroupGUIDs,@ObjectID) & @RequiredPermissionOnObject <> @RequiredPermissionOnObject )
		RETURN -100 -- Not enough permissions
		
	IF( dbo.[Folder_FindHighestUserPermission] (@UserGuid,@GroupGUIDs,@ObjectID) & @RequiredPermissionOnDestinationFolder <> @RequiredPermissionOnDestinationFolder )
		RETURN -100 -- Not enough permissions
	
	INSERT INTO	[Object_Folder_Join]([ObjectID],[FolderID],[ObjectFolderTypeID],[DateCreated])
         VALUES	(@ObjectID,
				 @FolderID,
				 @ObjectFolderID,
				 GETDATE())
				 
	RETURN @@ROWCOUNT
	
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.11.04
--				This SP return folders associated with an Object and the option of returning all parent folders too
-- =============================================
ALTER PROCEDURE [dbo].[Folder_Get]
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
						AND Object_Folder_Join.ObjectFolderTypeID = 1
			
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
				AND Object_Folder_Join.ObjectFolderTypeID = 1

	END
	
END
GO

