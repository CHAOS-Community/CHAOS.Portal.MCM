CREATE PROCEDURE ObjectRelationType_Update
(
    ID      INT,
    Name    VARCHAR(255)
)
BEGIN

    UPDATE
    	ObjectRelationType
	SET
		ObjectRelationType.Name = Name
	WHERE
		ObjectRelationType.ID = ID;
     
    SELECT ROW_COUNT();

END