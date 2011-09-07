-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.31
--				This SP is used to delete a folder
-- =============================================
CREATE PROCEDURE Folder_Delete
	@GroupGUIDs		GUIDList READONLY,
	@UserGUID		uniqueidentifier,
	@ID				int
AS
BEGIN

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'DELETE' )

	IF( dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,@ID ) & @RequiredPermission <> @RequiredPermission ) 
		RETURN -100

	IF EXISTS( SELECT * FROM Folder WHERE Folder.ParentID = @ID )
		RETURN -50
		
	IF EXISTS( SELECT * FROM Object_Folder_Join WHERE FolderID = @ID )
		RETURN -50
		
	BEGIN TRANSACTION Delete_Folder
	
	DELETE 
	  FROM	[Folder_Group_Join]
     WHERE	FolderID = @ID

	DELETE 
	  FROM	[Folder_User_Join]
     WHERE	FolderID = @ID
     
     DELETE 
       FROM	[Folder]
      WHERE	ID = @ID

	IF( @@ERROR = 0 )
		COMMIT TRANSACTION Delete_Folder
	ELSE
		ROLLBACK TRANSACTION Delete_Folder
		
	RETURN 1
	
END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.08.26
--				This SP is used to get a FolderInfo, if no search criteria are given it will find the top most folders
-- =============================================
ALTER PROCEDURE [dbo].[FolderInfo_Get] 
	@GroupGUIDs		GUIDList READONLY,
	@UserGUID		uniqueidentifier,
	@FolderID		int = null,
	@FolderTypeID	int = null,
	@ParentID		int = null
AS
BEGIN

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'GET' )
	
	IF( @FolderID IS NULL AND @ParentID IS NULL )
	BEGIN
		SELECT	*
		  FROM	FolderInfo
		 WHERE  dbo.Folder_IsFolderHighestLevel(@UserGUID,@GroupGUIDs,FolderInfo.ID) = 1 AND
				dbo.[Folder_FindHighestUserPermission] (@UserGUID,@GroupGUIDs,FolderInfo.ID) & @RequiredPermission = @RequiredPermission AND
				( @FolderTypeID IS NULL OR @FolderTypeID = FolderInfo.FolderTypeID )
	END
	ELSE
	BEGIN
		SELECT	*
		  FROM	FolderInfo
		 WHERE  dbo.[Folder_FindHighestUserPermission] (@UserGUID,@GroupGUIDs,FolderInfo.ID) & @RequiredPermission = @RequiredPermission AND
				( @FolderID IS NULL OR @FolderID = FolderInfo.ID ) AND
				( @FolderTypeID IS NULL OR @FolderTypeID = FolderInfo.FolderTypeID ) AND
				( @ParentID IS NULL OR @ParentID = FolderInfo.ParentID )
	END

END
GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.06
--				This SP is used to update a folder
-- =============================================
CREATE PROCEDURE [dbo].[Folder_Update]
	@GroupGUIDs			GUIDList READONLY,
	@UserGUID			uniqueidentifier,
	@ID					int,
	@NewTitle			varchar(255) = null,
	@NewParentID		int          = null,
	@NewFolderTypeID	int			 = null
AS
BEGIN

	if( @NewTitle IS NULL AND @NewFolderTypeID IS NULL AND @NewParentID IS NULL )
		RETURN -10

	DECLARE	@RequiredPermission	int
	SET @RequiredPermission = 0
	
	-- OR with general UPDATE permission if applies
	if( @NewTitle IS NOT NULL OR @NewFolderTypeID IS NOT NULL )
		SET @RequiredPermission = @RequiredPermission | dbo.GetPermissionForAction( 'Folder', 'UPDATE' )
	
	-- OR with MOVE permission if applies
	if( @NewParentID IS NOT NULL )
		SET @RequiredPermission = @RequiredPermission | dbo.GetPermissionForAction( 'Folder', 'MOVE' )
	
	IF( dbo.[Folder_FindHighestUserPermission]( @UserGUID,@GroupGUIDs,@ID ) & @RequiredPermission <> @RequiredPermission ) 
		RETURN -100

	UPDATE	Folder
	   SET	[ParentID] = @NewParentID
			,[FolderTypeID] = ISNULL(@NewFolderTypeID,[FolderTypeID])
			,[Title] = ISNULL(@NewTitle,[Title])
	 WHERE	Folder.ID = @ID

	RETURN @@ROWCOUNT
	
END

GO

-- =============================================
-- Author:		Jesper Fyhr Knudsen
-- Create date: 2011.09.06
--				This SP is used to create folders
-- =============================================
CREATE PROCEDURE [dbo].[Folder_Create]
	@GroupGUIDs				GUIDList READONLY,
	@UserGUID				uniqueidentifier,
	@SubscriptionGUID		uniqueidentifier	= null,
	@Title					varchar(255),
	@ParentID				int					= null,
	@FolderTypeID			int,
	@SubscriptionPermission	int					= null
AS
BEGIN

	DECLARE	@RequiredPermission	int
	
	-- If SubscriptionGUID is NOT NULL ParentID must be null, ELSE SubscriptionGUID is inherited from the parent
	IF( @SubscriptionGUID IS NULL AND @ParentID IS NULL AND @SubscriptionPermission IS NULL )
		RETURN -10
	
	IF( @ParentID IS NULL )
	BEGIN
		-- If ParentID is null, check permission on subscription
		SET @RequiredPermission = dbo.GetPermissionForAction( 'Subscription', 'CREATE TOPFOLDER' )
		
		IF( @RequiredPermission & @SubscriptionPermission <> @RequiredPermission )
			RETURN -100
	END
	ELSE
	BEGIN
		-- Check Create permission to ParentID
		SET @RequiredPermission = dbo.GetPermissionForAction( 'Folder', 'CREATE' )
		
		IF( @RequiredPermission & dbo.Folder_FindHighestUserPermission( @UserGUID,@GroupGUIDs,@ParentID ) <> @RequiredPermission )
			RETURN -100
	END

	IF( @SubscriptionGUID IS NULL AND @ParentID IS NOT NULL )
		SELECT	@SubscriptionGUID = SubscriptionGUID 
		  FROM	Folder
		 WHERE	Folder.ID = @ParentID
	
	BEGIN TRANSACTION Create_Folder
	
	INSERT INTO	[Folder] ([ParentID] ,[FolderTypeID] ,[SubscriptionGUID] ,[Title] ,[DateCreated])
         VALUES (@ParentID ,@FolderTypeID ,@SubscriptionGUID ,@Title ,GETDATE())
    
    DECLARE @FolderID INT
    SET @FolderID = @@IDENTITY       
    
    INSERT INTO [Folder_User_Join] ([FolderID],[UserGUID],[Permission],[DateCreated])
         VALUES	(@FolderID,@UserGUID,0x7FFFFFFF,GETDATE())
        
    IF( @@ERROR = 0 )
		COMMIT TRANSACTION Create_Folder
	ELSE
		ROLLBACK TRANSACTION Create_Folder

	RETURN @FolderID
END
GO
