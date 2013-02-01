CREATE PROCEDURE Language_Create
(
    Name			VARCHAR(255),
    LanguageCode	VARCHAR(10)
)
BEGIN

    INSERT INTO Language 
    	( Name, LanguageCode )
    VALUES
    	( Name,  LanguageCode );

    SELECT ROW_COUNT();

END