CREATE PROCEDURE Folder_Create
(
	UserGuid			BINARY(16),
	SubscriptionGuid	BINARY(16),
	Name				VARCHAR(255),
	ParentID			INT,
	FolderTypeID		INT
)
BEGIN

    DECLARE FolderID INT;
    DECLARE EXIT HANDLER
    FOR SQLEXCEPTION, SQLWARNING, NOT FOUND
    BEGIN
        ROLLBACK;
        SELECT -200;
    END;

    -- If SubscriptionGUID is NOT NULL ParentID must be null, ELSE SubscriptionGUID is inherited from the parent
    IF( SubscriptionGuid IS NULL AND ParentID IS NULL ) THEN
        SELECT -10;
    END IF;
    
    IF( UserGuid IS NULL ) THEN
        SELECT -10;
    END IF;

    START TRANSACTION;

        INSERT INTO	Folder
            ( ParentID, FolderTypeID, SubscriptionGUID, Name, DateCreated )
        VALUES
            ( ParentID, FolderTypeID, SubscriptionGuid, Name, NOW() );

        SET FolderID = last_insert_id();     

        INSERT INTO Folder_User_Join 
            ( FolderID, UserGUID, Permission, DateCreated )
        VALUES	
            ( FolderID, UserGuid, 4294967295, NOW() );
        
    COMMIT;

    SELECT FolderID;

END