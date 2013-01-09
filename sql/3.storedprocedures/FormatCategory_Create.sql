CREATE PROCEDURE FormatCategory_Create
(    
	IN  FormatTypeID    INT,
    IN  Name            VARCHAR(255)
)
BEGIN

    INSERT INTO FormatCategory
    	( FormatTypeID, Name )
    VALUES
    	( FormatTypeID, Name );
           
    SELECT last_insert_id();

END