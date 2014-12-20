CREATE PROCEDURE File_Get
(
    ID  INT,
	ParentId INT
)
BEGIN

	SELECT  
		File.*
	FROM  
		File
	WHERE  
			(ID IS NULL OR File.ID = ID) 
		AND (ParentId IS NULL OR File.ParentID = ParentId);

END