CREATE PROCEDURE ObjectType_Set
(
    ID      INT,
    Name    VARCHAR(255)
)
BEGIN

	IF(ID IS NOT NULL) THEN

		IF NOT EXISTS(SELECT * FROM ObjectType WHERE ObjectType.ID = ID) THEN

			INSERT INTO ObjectType
				( ID, Name )
			VALUES
				( ID, Name );

			SELECT last_insert_id();

		ELSE 

			UPDATE 
				ObjectType
			SET 
				ObjectType.Name = Name
			WHERE 
				ObjectType.ID = ID;
		 
			 SELECT ID;

		 END IF;

	ELSE

		INSERT INTO ObjectType
			( Name )
		VALUES
			( Name );

		SELECT last_insert_id();

	END IF;

END