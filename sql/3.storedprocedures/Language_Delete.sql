CREATE PROCEDURE Language_Delete
(
    LanguageCode        VARCHAR(10)
)
BEGIN

    DELETE FROM Language
    WHERE
    	Language.LanguageCode = LanguageCode;

    SELECT ROW_COUNT();

END