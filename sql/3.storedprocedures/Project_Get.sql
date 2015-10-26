CREATE PROCEDURE `Project_Get` 
(
	Id		INT UNSIGNED,
    LabelId	INT UNSIGNED,
	UserId	BINARY(16)
)
BEGIN

	SELECT DISTINCT
		P.*
	FROM
		Project AS P
		LEFT JOIN Label AS L ON P.Id = L.ProjectId
        LEFT JOIN Project_User_Join AS PUJ ON P.Id = PUJ.ProjectId
	WHERE
			(Id IS NULL OR P.Id = Id) 
        AND	(LabelId IS NULL OR L.Id = LabelId)
        AND (UserId IS NULL OR PUJ.UserId = UserId);

END
