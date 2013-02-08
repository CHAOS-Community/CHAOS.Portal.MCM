CREATE PROCEDURE DestinationInfo_Get
(
    DestinationID   INT UNSIGNED
)
BEGIN

    SELECT	
    	*
      FROM	
      	DestinationInfo
     WHERE	
     	DestinationInfo.ID = DestinationID;

END