CREATE PROCEDURE FormatCategory_Get
(
    ID		INT,
    Name	VARCHAR(255)
)
BEGIN

    SELECT	
    	*
    FROM
    	FormatCategory
    WHERE
    		( ID   IS NULL OR FormatCategory.ID = ID )
        AND ( Name IS NULL OR FormatCategory.Name = Name );
            
END