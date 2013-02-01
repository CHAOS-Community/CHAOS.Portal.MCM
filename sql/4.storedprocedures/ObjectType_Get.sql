CREATE PROCEDURE ObjectType_Get
(
    ID      INT,
    Name    VARCHAR(255)
)
BEGIN

    SELECT
    	*
	FROM
		ObjectType
	WHERE
			( ID   IS NULL OR ObjectType.ID   = ID )
        AND	( Name IS NULL OR ObjectType.Name = Name );

END