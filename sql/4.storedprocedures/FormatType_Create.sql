CREATE PROCEDURE FormatType_Create
(
    Name    VARCHAR(255)
)
BEGIN


    INSERT INTO FormatType
    	( Name )
    VALUES
    	( Name );
           
    SELECT last_insert_id();

END