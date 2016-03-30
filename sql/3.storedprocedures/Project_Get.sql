CREATE PROCEDURE `Project_Get` 
(
	Id		INT UNSIGNED,
    LabelId	INT UNSIGNED,
	UserId	BINARY(16)
)
BEGIN

	CREATE TEMPORARY TABLE IF NOT EXISTS ObjectGUID_Table 
    (
        Id    INT UNSIGNED NOT NULL
    );

    DELETE FROM ObjectGUID_Table;

	INSERT INTO ObjectGUID_Table
		SELECT DISTINCT
			P.Id
		FROM
			Project AS P
			LEFT JOIN Label AS L ON P.Id = L.ProjectId
			LEFT JOIN Project_User_Join AS PUJ ON P.Id = PUJ.ProjectId
		WHERE
				(Id IS NULL OR P.Id = Id) 
			AND	(LabelId IS NULL OR L.Id = LabelId)
			AND (UserId IS NULL OR PUJ.UserId = UserId);

	SELECT
		P.*
	FROM
		ObjectGUID_Table
		INNER JOIN Project AS P ON P.Id = ObjectGUID_Table.Id;

	SELECT
		L.*
	FROM
		ObjectGUID_Table
		INNER JOIN Label AS L ON L.ProjectId = ObjectGUID_Table.Id;

	SELECT
		PUJ.*
	FROM
		ObjectGUID_Table
		INNER JOIN Project_User_Join AS PUJ ON PUJ.ProjectId = ObjectGUID_Table.Id;
END
