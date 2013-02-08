CREATE VIEW DestinationInfo 
AS 
	SELECT 
		Destination.ID AS ID, 
		Destination.SubscriptionGUID AS SubscriptionGUID, 
		Destination.Name AS Name, 
		Destination.DateCreated AS DateCreated, 
		AccessProvider.BasePath AS BasePath, 
		AccessProvider.StringFormat AS StringFormat, 
		AccessProvider.Token AS Token 
	FROM
		Destination 
		INNER JOIN AccessProvider ON Destination.ID = AccessProvider.DestinationID;
