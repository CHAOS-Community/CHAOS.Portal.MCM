CREATE PROCEDURE Folder_Create
(
    IN  UserGUID                BINARY(16),
    IN  SubscriptionGUID        BINARY(16),
    IN  Name                    VARCHAR(255),
    IN  ParentID                INT,
    IN  FolderTypeID            INT
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
    IF( SubscriptionGUID IS NULL AND ParentID IS NULL ) THEN
        SELECT -10;
    END IF;
    
    IF( UserGUID IS NULL ) THEN
        SELECT -10;
    END IF;

    START TRANSACTION;

        INSERT INTO	Folder
            ( ParentID, FolderTypeID, SubscriptionGUID, Name, DateCreated )
        VALUES
            ( ParentID, FolderTypeID, SubscriptionGUID, Name, NOW() );

        SET FolderID = last_insert_id();     

        INSERT INTO Folder_User_Join 
            ( FolderID, UserGUID, Permission, DateCreated )
        VALUES	
            ( FolderID, UserGUID, 4294967295, NOW() );
        
    COMMIT;

    SELECT FolderID;

END