CREATE PROCEDURE ObjectRelationType_Get
(
    ID		INT,
    Name	VARCHAR(255)
)
BEGIN

    SELECT
    	*
	FROM
		ObjectRelationType
	WHERE
			( ID   IS NULL OR ObjectRelationType.ID = ID )
        AND ( Name IS NULL OR ObjectRelationType.Name = Name );

END