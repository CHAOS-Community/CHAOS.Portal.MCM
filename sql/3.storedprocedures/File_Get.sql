CREATE PROCEDURE File_Get
(
    ID  INT
)
BEGIN

	SELECT  
		File.*
	FROM  
		File
	WHERE  
		File.ID = ID;

END