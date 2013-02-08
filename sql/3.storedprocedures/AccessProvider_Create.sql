CREATE PROCEDURE AccessProvider_Create
(
    DestinationID   INT UNSIGNED, 
    BasePath        VARCHAR(2048), 
    StringFormat    VARCHAR(2048),  
    Token           VARCHAR(255)
)
BEGIN
    INSERT INTO AccessProvider
    	( DestinationID, BasePath, StringFormat, DateCreated, Token ) 
    VALUES
    	( DestinationID, BasePath, StringFormat, NOW()      , Token );
                        
    SELECT last_insert_id();
END