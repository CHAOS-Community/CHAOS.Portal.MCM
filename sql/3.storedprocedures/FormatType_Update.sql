CREATE PROCEDURE FormatType_Update
(
    ID      INT,
    Name    VARCHAR(255)
)
BEGIN

    UPDATE	
    	FormatType
    SET	
    	FormatType.Name = Name
    WHERE
    	FormatType.ID = ID;
     
    SELECT ROW_COUNT();

END