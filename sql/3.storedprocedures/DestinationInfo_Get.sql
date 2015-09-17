CREATE PROCEDURE DestinationInfo_Get
(
    ID   INT UNSIGNED
)
BEGIN

    SELECT	
    	*
      FROM	
      	DestinationInfo
     WHERE	
     	( ID IS NULL OR DestinationInfo.ID = ID );

END