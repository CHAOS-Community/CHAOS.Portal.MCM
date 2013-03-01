CREATE PROCEDURE ObjectType_Set
(
    ID      INT,
    Name    VARCHAR(255)
)
BEGIN

	IF(ID IS NULL) THEN

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

END