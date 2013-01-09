CREATE PROCEDURE FormatType_Delete
(
    ID	INT
)
BEGIN

    DELETE FROM
    	FormatType
    WHERE
    	FormatType.ID = ID;
     
    SELECT ROW_COUNT();
END