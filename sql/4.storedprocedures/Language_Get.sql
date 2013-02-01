CREATE PROCEDURE Language_Get
(
    Name            VARCHAR(255),
    LanguageCode    VARCHAR(10)
)
BEGIN

	SELECT
		*
	FROM
		Language
	WHERE
			( Name         IS NULL OR Language.Name = Name )
        AND ( LanguageCode IS NULL OR Language.LanguageCode = LanguageCode );

END