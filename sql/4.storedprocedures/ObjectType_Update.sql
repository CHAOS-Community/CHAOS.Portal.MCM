CREATE PROCEDURE ObjectType_Update
(
    ID      INT,
    Name    VARCHAR(255)
)
BEGIN
        
    UPDATE 
    	ObjectType
	SET 
		ObjectType.Name = Name
	WHERE 
		ObjectType.ID = ID;
     
     SELECT ROW_COUNT();

END