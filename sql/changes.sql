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

	INSERT INTO [Object_Object_Join] ([ObjectID1],[ObjectID2],[ObjectRelationTypeID],[Sequence],[DateCreated])
		 VALUES (@Object1ID,@Object2ID,@ObjectRelationTypeID,@Sequence,GETDATE())
		 
	RETURN @@ROWCOUNT

END
GO