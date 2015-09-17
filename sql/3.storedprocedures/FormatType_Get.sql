CREATE PROCEDURE FormatType_Get
(
    ID		INT,
    Name	VARCHAR(255)
)
BEGIN

    SELECT	
    	*
	FROM	
		FormatType
    WHERE
    		( ID   IS NULL OR FormatType.ID = ID )
        AND ( Name IS NULL OR FormatType.Name = Name );
            
END