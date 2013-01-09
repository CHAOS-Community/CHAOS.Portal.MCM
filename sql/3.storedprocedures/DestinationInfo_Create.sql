CREATE PROCEDURE Destination_Create
(
    SubscriptionGUID    BINARY(16), 
    Name                VARCHAR(255)
)
BEGIN

    INSERT INTO Destination
    	( SubscriptionGUID, Name, DateCreated ) 
    VALUES
    	( SubscriptionGUID, Name, NOW() );
                         
    SELECT last_insert_id();

END