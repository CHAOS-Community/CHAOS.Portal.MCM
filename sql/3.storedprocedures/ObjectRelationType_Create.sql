CREATE PROCEDURE ObjectRelationType_Create
(
    Name    VARCHAR(255)
)
BEGIN

    INSERT INTO ObjectRelationType
    	( Name )
	VALUES
		( Name );
           
    SELECT last_insert_id();

END