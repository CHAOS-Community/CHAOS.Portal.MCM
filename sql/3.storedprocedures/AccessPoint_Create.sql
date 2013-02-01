CREATE PROCEDURE AccessPoint_Create
(
    GUID                BINARY(16),
    SubscriptionGuid    BINARY(16),
    Name                VARCHAR(255)
)
BEGIN

    INSERT INTO AccessPoint 
    	(GUID, SubscriptionGuid, Name, DateCreated)
    VALUES 
        (GUID, SubscriptionGuid, Name, NOW());

    SELECT ROW_COUNT();

END