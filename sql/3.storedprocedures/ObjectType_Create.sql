CREATE PROCEDURE ObjectType_Create
(
    Name   VARCHAR(255)
)
BEGIN

    INSERT INTO ObjectType
    	( Name )
	VALUES
		( Name );

    SELECT last_insert_id();

END