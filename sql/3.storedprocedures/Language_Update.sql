CREATE PROCEDURE Language_Update
(
    Name			VARCHAR(255),
    LanguageCode	VARCHAR(10)
)
BEGIN

    UPDATE	
    	Language
	SET	
		Language.Name = Name
	WHERE	
		Language.LanguageCode = LanguageCode;

    SELECT ROW_COUNT();

END