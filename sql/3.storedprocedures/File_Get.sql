CREATE PROCEDURE File_Get
(
    FileID  INT
)
BEGIN

	SELECT  
		File.*
	FROM  
		File
	WHERE  
		File.ID = FileID;

END